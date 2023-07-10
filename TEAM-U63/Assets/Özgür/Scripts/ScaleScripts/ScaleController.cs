using System;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// <para>Responsible of scale's position, colors, completion status and more</para>
/// </summary>
public class ScaleController : MonoBehaviour
{
    private static int completedScaleNumber;
    private static bool isAllScalesCompleted;
    public static event Action<bool> OnScaleCompleted;
    
    [Header("Assign - NetworkParentListID")]
    [SerializeField] private int networkParentListID;

    //moveSpeed, completionLocalPositionY and maxLocalPosition should be static
    [Header("Assign")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float completionLocalPositionY = -4.5f;
    [SerializeField] private float maxLocalPosition = -4.7f;
    [SerializeField] private Transform ceilingTransform;
    //TODO: Remove after tests are done
    [SerializeField] private bool testRunScale;

    //Materials should be static but we can't assign static variables in inspector, assign the same for every scale
    //Ceiling mesh renderer is object specific though
    [Header("Assign - Materials")]
    [SerializeField] private Material completedMaterial;
    [SerializeField] private Material notCompletedMaterial;
    [SerializeField] private MeshRenderer ceilingMeshRenderer;
    
    //These should be static
    [Header("Assign - Color multipliers")]
    [SerializeField] private float redMultiplier = 0.5f;
    [SerializeField] private float greenMultiplier = 0.4f;
    [SerializeField] private float blueMultiplier = 0.8f;
    
    [Header("Number of cubes")]
    [SerializeField] private int redNumber;
    [SerializeField] private int greenNumber;
    [SerializeField] private int blueNumber;
    
    [Header("isCompleted")]
    [SerializeField] private bool isCompleted;

    private LineRenderer lr;
    private Vector3 fixedPosition;
    private Vector3 weightlessPosition;
    private float weightlessLocalPositionY;
    
    private CubeManager enteredCubeManager;   //Explanation is in further down where it's being used
    private bool isEnteredCubeStatesNull = true;        //Comparison to null is expensive

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();

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
    }

    private void Update()
    {
        //TODO: Remove after tests are done
        if (testRunScale)
        {
            UpdateScalePosition();
            testRunScale = false;
        }
        
        //Not the best way i think
        lr.SetPosition(1, transform.position + new Vector3(0f, 0.5f, 0f));
    }

    /// <summary>
    /// <para>Updates scale's position when the weights changes</para>
    /// </summary>
    private void UpdateScalePosition()
    {
        float stretchAmount = redNumber * redMultiplier + greenNumber * greenMultiplier + blueNumber * blueMultiplier;
        float localPositionYAfterStretch = weightlessLocalPositionY - stretchAmount;
        
        //Local positions are negative, so smaller position means longer lenght
        if (localPositionYAfterStretch < maxLocalPosition)
            stretchAmount -=  math.abs(localPositionYAfterStretch - maxLocalPosition);
        
        //To set a constant speed for DOMove, no matter what the distance is, we need these calculations //TYT FİZİK
        float targetPositionY = weightlessPosition.y - stretchAmount;
        float distance = math.abs(transform.position.y - targetPositionY);
        float duration = distance / moveSpeed;
        
        transform.DOMoveY(targetPositionY, duration).SetEase(Ease.Linear);
        
        //We need to check completion after DOMoveY method is done because we are comparing transform.position.y..
        //..in that method. Since DOMoveY method is a coroutine, transform.position.y will change during "duration"
        Invoke(nameof(CheckCompletion), duration + 0.1f);
    }
    
    /// <summary>
    /// <para>Checks if the scale reached the lenght needed for completion</para>
    /// </summary>
    private void CheckCompletion()
    {
        float currentLocalPositionY =  transform.localPosition.y;

        if (math.abs(currentLocalPositionY - completionLocalPositionY) < 0.01f)    //Float comparison is not precise
        {
            isCompleted = true;
            completedScaleNumber++;
        }
        
        else if (math.abs(currentLocalPositionY - completionLocalPositionY) > 0.01f && isCompleted) //Float comparison is not precise
        {
            isCompleted = false;
            completedScaleNumber--;
        }

        else
        {
            isCompleted = false;
        }
        
        isAllScalesCompleted = completedScaleNumber == 3;
        
        UpdateCompletionMaterial();

        //TODO: Remove after tests are done
        //Debug.Log(gameObject.name + ": " + isCompleted);
        //Debug.Log("how many completed: " + completedScaleNumber);
        //Debug.Log("all completed: " + isAllScalesCompleted);

        OnScaleCompleted?.Invoke(isAllScalesCompleted);
    }

    /// <summary>
    /// <para>Updates the material of the ceiling according to completion</para>
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
        
        //Also the parenting in this script cause conflict with PlayerGrabbing.cs parenting. It must also be
        //..done while player is not holding the cube.
        
        //To do all of that, we need the CubeManager.cs from the cube that has entered the collider...
        //..and check if it's currently held by player or not. That state is updated by PlayerGrabbing.cs
        enteredCubeManager = col.GetComponent<CubeManager>();
        isEnteredCubeStatesNull = false;    //Comparison to null is expensive, we will check that variable instead
    }

    private void FixedUpdate()
    {
        if (isEnteredCubeStatesNull) return; 
        if (enteredCubeManager.isGrabbed) return;
        
        //Parenting is needed for smooth movement and good looking motion
        enteredCubeManager.UpdateParentUsingNetworkParentListID(networkParentListID);

        UpdateScalePosition();
        
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
        
        UpdateScalePosition();
    }
}