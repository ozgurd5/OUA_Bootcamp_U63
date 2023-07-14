using System;
using DG.Tweening;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// <para>Responsible of scale's position, colors, completion status and more</para>
/// </summary>
public class ScaleController : MonoBehaviour
{
    private static bool isAllScalesCompleted;
    public static event Action<bool> OnScaleCompleted;

    //TODO: find a better solution
    private static bool[] scaleCompletionStatus = new bool[3];

    [Header("Assign - NetworkParentListID")]
    [SerializeField] private int networkParentListID;

    //moveSpeed, completionLocalPositionY and maxLocalPosition should be static
    [Header("Assign")]
    [SerializeField] private BottomDetector bottomDetector;
    [SerializeField] private Transform ceilingTransform;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float completionLocalPositionY = -4.2f;
    [SerializeField] private float maxLocalPosition = -4.7f;

    //Materials should be static but we can't assign static variables in inspector, assign the same for every scale
    //Ceiling mesh renderer is object specific though
    [Header("Assign - Materials")]
    [SerializeField] private Material completedMaterial;
    [SerializeField] private Material notCompletedMaterial;
    [SerializeField] private MeshRenderer ceilingMeshRenderer;
    
    //These too should be static
    [Header("Assign - Audio")]
    [SerializeField] private AudioSource aus;
    [SerializeField] private AudioClip scaleCompletedClip;
    [SerializeField] private AudioClip allScalesCompletedClip;
    
    //These too should be static
    [Header("Assign - Color multipliers")]
    [SerializeField] private float redMultiplier = 0.3f;
    [SerializeField] private float greenMultiplier = 0.4f;
    [SerializeField] private float blueMultiplier = 0.5f;
    
    [Header("Number of cubes")]
    [SerializeField] private int redNumber;
    [SerializeField] private int greenNumber;
    [SerializeField] private int blueNumber;
    [SerializeField] private bool testRunScale;
    
    [Header("Info - No Touch")]
    [SerializeField] private bool isCompleted;

    private TextMeshProUGUI weightText;
    private LineRenderer lr;
    private Vector3 fixedPosition;
    private Vector3 weightlessPosition;
    private float weightlessLocalPositionY;
    
    private CubeManager enteredCubeManager;         //Explanation is in further down where it's being used
    private bool isEnteredCubeStatesNull = true;    //Comparison to null is expensive

    private Tween movementTween;
    private bool isMovingDown;
    private bool isMovingUp;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        weightText = GetComponentInChildren<TextMeshProUGUI>();

        //Scales are weightless in the beginning
        weightlessPosition = transform.position;
        weightlessLocalPositionY = transform.localPosition.y;
        
        //Fixed and never changing position of the line's top end is the ceiling 
        fixedPosition = new Vector3(weightlessPosition.x, ceilingTransform.position.y, weightlessPosition.z);
        
        //Line renderer is disabled and it's default positions are (0,0,0) in both end by default, we must change it
        lr.enabled = true;
        lr.SetPosition(0, fixedPosition);
        lr.SetPosition(1, weightlessPosition);

        //Assign materials in the beginning
        UpdateCompletionMaterial();
        
        bottomDetector.OnBottomExit += UpdateScalePosition;
    }

    private void Update()
    {
        if (testRunScale)
        {
            UpdateScalePosition();
            testRunScale = false;
        }

        //If there is an object bellow the scale and it tries to go down, it must not go down anymore
        if (bottomDetector.isTouching && isMovingDown) movementTween.Kill(); 
        
        //Not the best way i think
        lr.SetPosition(1, transform.position + new Vector3(0f, 0.7f, 0f));
    }

    /// <summary>
    /// <para>Updates scale's position when the weights changes</para>
    /// </summary>
    private void UpdateScalePosition()
    {
        float stretchAmount = redNumber * redMultiplier + greenNumber * greenMultiplier + blueNumber * blueMultiplier;
        float localPositionYAfterStretch = weightlessLocalPositionY - stretchAmount;
        
        //Local positions are negative, so smaller y value means longer lenght
        if (localPositionYAfterStretch < maxLocalPosition)
            stretchAmount -=  math.abs(localPositionYAfterStretch - maxLocalPosition);
        
        //To set a constant speed for DOMove, no matter what the distance is, we need these calculations //TYT FİZİK
        float targetPositionY = weightlessPosition.y - stretchAmount;
        float distance = math.abs(transform.position.y - targetPositionY);
        float duration = distance / moveSpeed;
        
        //isMovingDown and movementTween is necessary for stopping when there is an object bellow the scale
        isMovingDown = transform.localPosition.y > localPositionYAfterStretch;
        movementTween = transform.DOMoveY(targetPositionY, duration).SetEase(Ease.Linear);
        
        //We need to check completion after DOMoveY method is done because we are comparing transform.position.y..
        //..in that method. Since DOMoveY method is a coroutine, transform.position.y will change during "duration"
        Invoke(nameof(CheckCompletion), duration + 0.02f);
        
        //Same thing applies for these too
        Invoke(nameof(UpdateWeightText), duration + 0.02f);
        Invoke(nameof(SetIsMovingDownFalse), duration + 0.02f);
    }

    /// <summary>
    /// <para>Sets isMovingDown to false</para>
    /// <para>Must be called in UpdateScalePosition</para>
    /// </summary>
    private void SetIsMovingDownFalse()
    {
        isMovingDown = false;
    }

    /// <summary>
    /// <para>Updates weight text above the ceiling</para>
    /// <para>Must be called in UpdateScalePosition</para>
    /// </summary>
    private void UpdateWeightText()
    {
        if (bottomDetector.isTouching) weightText.text = ":(";
        else weightText.text = $"{(int)(math.abs(weightlessLocalPositionY - transform.localPosition.y + 0.001f) * 10)}";
    }
    
    /// <summary>
    /// <para>Checks if the scale reached the lenght needed for completion</para>
    /// <para>Must be called in UpdateScalePosition</para>
    /// </summary>
    private void CheckCompletion()
    {
        float currentLocalPositionY =  transform.localPosition.y;

        //Float comparison is not precise
        if (math.abs(currentLocalPositionY - completionLocalPositionY) < 0.01f) isCompleted = true;
        else if (math.abs(currentLocalPositionY - completionLocalPositionY) > 0.01f && isCompleted) isCompleted = false;
        else isCompleted = false;

        //TODO: find a better solution
        scaleCompletionStatus[networkParentListID - 1] = isCompleted;
        isAllScalesCompleted = scaleCompletionStatus[0] && scaleCompletionStatus[1] && scaleCompletionStatus[2];
        
        if (isAllScalesCompleted) aus.PlayOneShot(allScalesCompletedClip);
        else if (isCompleted) aus.PlayOneShot(scaleCompletedClip);
        
        UpdateCompletionMaterial();
        OnScaleCompleted?.Invoke(isAllScalesCompleted);
    }

    /// <summary>
    /// <para>Updates the material of the ceiling according to completion</para>
    /// <para>Must be called in CheckCompletion</para>
    /// </summary>
    private void UpdateCompletionMaterial()
    {
        //Updating mesh renderer materials in Unity is ultra protected for several long reasons
        //Long story short: We can not change a single element of the mesh renderer's materials array
        //We can only change the complete array by assign an array to it
        //So we must make our changes in a temporary copy array and assign it to mesh renderer material

        Material[] newCeilingMeshRendererMaterials = ceilingMeshRenderer.materials;

        if (isCompleted)
        {
            newCeilingMeshRendererMaterials[1] = completedMaterial;
            lr.material = completedMaterial;
        }
        else
        {
            newCeilingMeshRendererMaterials[1] = notCompletedMaterial;
            lr.material = notCompletedMaterial;
        }

        ceilingMeshRenderer.materials = newCeilingMeshRendererMaterials;
    }
    
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("RedPuzzle"))
            redNumber++;
        else if (col.CompareTag("GreenPuzzle"))
            greenNumber++;
        else if (col.CompareTag("BluePuzzle"))
            blueNumber++;
        else
            return;

        //Updating scale position is not smooth and looks very bad while player is holding the cube
        //We can not check "if player is holding the cube" in OnTriggerEnter method. Player can drop..
        //..the cube after it's entry to the collider. So we must check it dynamically in FixedUpdate
        
        //Same thing applies to the parenting. We must not change the parent of the cube while player is..
        //..holding the cube. Grabbing mechanic needs parenting
        enteredCubeManager = col.GetComponent<CubeManager>();
        isEnteredCubeStatesNull = false;    //Comparison to null is expensive, we will check that variable instead
    }

    private void FixedUpdate()
    {
        if (isEnteredCubeStatesNull) return; 
        if (enteredCubeManager.isGrabbed) return;

        UpdateScalePosition();
        
        //Parenting is needed for smooth movement and good looking motion
        enteredCubeManager.UpdateParentUsingNetworkParentListID(networkParentListID);
        
        enteredCubeManager = null;
        isEnteredCubeStatesNull = true; //Comparison to null is expensive, we will check that variable instead
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("RedPuzzle"))
            redNumber--;
        else if (col.CompareTag("GreenPuzzle"))
            greenNumber--;
        else if (col.CompareTag("BluePuzzle"))
            blueNumber--;
        else
            return;
        
        //If the cube exit from collider naturally, not grabbing -for example dropping- we must set the parent of it..
        //..to null. If it's grabbed, parenting to null would create a conflict so we must avoid it then
        
        CubeManager exitedCubeManager = col.GetComponent<CubeManager>();
        if (!exitedCubeManager.isGrabbed) exitedCubeManager.UpdateParentUsingNetworkParentListID(-1);   //-1 means null
        
        UpdateScalePosition();
    }
}