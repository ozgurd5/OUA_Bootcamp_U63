using System;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

//TODO: Remove debugRunScale when tests are done

/// <summary>
/// <para>Controls each scale and their position</para>
/// </summary>
public class ScaleController : MonoBehaviour
{
    private static int completedScaleNumber;
    public static bool isAllScalesCompleted;

    //moveSpeed, completionLocalPositionY and maxLocalPosition should be static
    [Header("Assign")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float completionLocalPositionY = -4.5f;
    [SerializeField] private float maxLocalPosition = -4.7f;
    [SerializeField] private Transform ceilingTransform;
    //Remove after tests are done
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
    
    private CubeStateManager enteredCubeStateManager;   //Explanation is in further down where it's being used
    private bool isEnteredCubeStatesNull = true;        //Comparison to null is expensive

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();

        //Scales are weightless in the beginning
        weightlessPosition = transform.position;
        weightlessLocalPositionY = transform.localPosition.y;
        
        //Fixed and never changing position of the line's beginning in the ceiling
        fixedPosition = new Vector3(weightlessPosition.x, ceilingTransform.position.y, weightlessPosition.z);
        
        //Line renderer is disabled and it's default positions are (0,0,0) before we start the game
        lr.enabled = true;
        lr.SetPosition(0, fixedPosition);
        lr.SetPosition(1, weightlessPosition);

        //Assign materials in the beginning
        UpdateCompletionMaterial();
    }

    private void Update()
    {
        //Remove after tests are done
        if (testRunScale)
        {
            UpdateScalePosition();
            testRunScale = false;
        }
        
        //Not the best way :p
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
        
        //We need to check completion after DOMoveY is done because we are comparing transform.position.y..
        //..in that method. Since DOMoveY is a coroutine, transform.position.y will change during "duration"
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

        //Remove after tests are done
        //Debug.Log(gameObject.name + ": " + isCompleted);
        //Debug.Log("how many completed: " + completedScaleNumber);
        //Debug.Log("all completed: " + isAllScalesCompleted);
    }

    /// <summary>
    /// <para>Updates the material of the ceiling according to completion</para>
    /// </summary>
    private void UpdateCompletionMaterial()
    {
        //Updating mesh renderer materials in Unity is ultra protected for several long reasons
        //Long story short: We can not change a single element of the mesh renderer's materials array
        //We can only change the complete array by assign an array to it
        //So we must make our changes in a copy array and assign it to mesh renderer material

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
        
        //Parenting is needed for smooth movement and good looking motion
        col.transform.SetParent(transform);
        
        //Updating scale position is not smooth and looks very bad while player is holding the cube
        //We can not check if player is holding the cube in OnTriggerEnter method. Player can drop..
        //..the cube after it's entry to the collider. So we must check it dynamically in FixedUpdate
        //To do that, we need the data of the cube that has entered the collider if it's currently held by player or not
        enteredCubeStateManager = col.GetComponent<CubeStateManager>();
        isEnteredCubeStatesNull = false;    //Comparison to null is expensive
    }

    private void FixedUpdate()
    {
        if (isEnteredCubeStatesNull) return; 
        if (enteredCubeStateManager.isGrabbed) return;

        UpdateScalePosition();
        enteredCubeStateManager = null;
        isEnteredCubeStatesNull = true; //Comparison to null is expensive
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
        
        //Release the cube when it's taken back by player
        col.transform.SetParent(null);
        UpdateScalePosition();
    }
}