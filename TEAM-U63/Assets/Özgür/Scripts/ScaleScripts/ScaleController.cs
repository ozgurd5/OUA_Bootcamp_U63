using System;
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

    [Header("Assign")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float completionLenght = 14f;
    //Remove after tests are done
    [SerializeField] private bool debugRunScale;

    [Header("Assign - Materials")]
    [SerializeField] private Material completedMaterial;
    [SerializeField] private Material notCompletedMaterial;
    [SerializeField] private MeshRenderer ceilingMeshRenderer;
    
    [Header("Assign - Color multipliers")]
    [SerializeField] private float redMultiplier = 0.4f;
    [SerializeField] private float greenMultiplier = 0.3f;
    [SerializeField] private float blueMultiplier = 0.5f;
    
    [Header("Number of cubes")]
    [SerializeField] private int redNumber;
    [SerializeField] private int greenNumber;
    [SerializeField] private int blueNumber;
    
    [SerializeField] private bool isCompleted;

    private LineRenderer lr;
    private Vector3 fixedPosition;
    private Vector3 weightlessPosition;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();

        //Scales are weightless in the beginning
        weightlessPosition = transform.position;
        
        //Fixed and never changing position of the line's beginning in the ceiling
        fixedPosition = new Vector3(weightlessPosition.x, transform.parent.position.y, weightlessPosition.z);
        
        //Line renderer's default positions are 0 in the beginning
        lr.enabled = true;
        lr.SetPosition(0, fixedPosition);
        lr.SetPosition(1, weightlessPosition);

        //Assign materials in the beginning
        UpdateCompletionMaterial();
    }

    private void Update()
    {
        //Remove after tests are done
        if (debugRunScale)
        {
            UpdateScalePosition();
            debugRunScale = false;
        }
        
        //Not the best way :p
        lr.SetPosition(1, transform.position);
    }

    /// <summary>
    /// <para>Updates scale's position when the weights changes</para>
    /// </summary>
    private void UpdateScalePosition()
    {
        float stretchAmount = redNumber * redMultiplier + greenNumber * greenMultiplier + blueNumber * blueMultiplier;
        
        //To set a constant speed for DOMove, no matter what the distance is, we need these calculations //TYT FİZİK
        float targetPositionY = weightlessPosition.y - stretchAmount;
        float distance = math.abs(transform.position.y - targetPositionY);
        float duration = distance / moveSpeed;
        
        transform.DOMoveY(targetPositionY, duration).SetEase(Ease.Linear);
        
        //DOMoveY is a coroutine, we need do check after it is done because we are comparing transform.position.y
        //Since DOMoveY is a coroutine, transform.position.y will change during "duration"
        Invoke(nameof(CheckCompletion), duration + 0.1f);
        Invoke(nameof(UpdateCompletionMaterial), duration + 0.1f);
    }
    
    /// <summary>
    /// <para>Checks if the scale reached the lenght needed for completion</para>
    /// </summary>
    private void CheckCompletion()
    {
        float currentLenght = transform.localPosition.y;
        
        if (math.abs(currentLenght * -1 - completionLenght) < 0.01f)    //Float comparison is not precise
        {
            isCompleted = true;
            completedScaleNumber++;
        }
        
        else if (Math.Abs(currentLenght - completionLenght) > 0.01f && isCompleted) //Float comparison is not precise
        {
            isCompleted = false;
            completedScaleNumber--;
        }

        else
        {
            isCompleted = false;
        }
        
        isAllScalesCompleted = completedScaleNumber == 3;
        
        //Remove after tests are done
        Debug.Log(gameObject.name + ": " + isCompleted);
        Debug.Log("how many completed: " + completedScaleNumber);
        Debug.Log("all completed: " + isAllScalesCompleted);
    }

    /// <summary>
    /// <para>Updates the material of the ceiling according to completion</para>
    /// </summary>
    private void UpdateCompletionMaterial()
    {
        if (isCompleted)
        {
            ceilingMeshRenderer.materials[1] = completedMaterial;
            lr.material = completedMaterial;
        }
        else
        {
            ceilingMeshRenderer.materials[1] = notCompletedMaterial;
            lr.material = notCompletedMaterial;
        }
    }
    
    private void OnTriggerEnter(Collider col)
    {
        //Set the cube child of the scale for smooth movement
        col.transform.SetParent(transform);
        
        if (col.CompareTag("RedPuzzle"))
            redNumber++;
        else if (col.CompareTag("GreenPuzzle"))
            greenNumber++;
        else if (col.CompareTag("BluePuzzle"))
            blueNumber++;

        UpdateScalePosition();
    }

    private void OnTriggerExit(Collider col)
    {
        //Release the cube if it's taken back
        col.transform.SetParent(null);
        
        if (col.CompareTag("RedPuzzle"))
            redNumber--;
        else if (col.CompareTag("GreenPuzzle"))
            greenNumber--;
        else if (col.CompareTag("BluePuzzle"))
            blueNumber--;

        UpdateScalePosition();
    }
}