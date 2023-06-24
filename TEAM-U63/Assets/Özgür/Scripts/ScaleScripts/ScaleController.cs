using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

//TODO: Remove debugRunScale when tests are done

/// <summary>
/// <para>Controls each scale and their position</para>
/// </summary>
public class ScaleController : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private float moveSpeed = 2f;
    //
    [SerializeField] private bool debugRunScale;
    
    [Header("Assign - Color multipliers")]
    [SerializeField] private float redMultiplier = 0.2f;
    [SerializeField] private float greenMultiplier = 0.1f;
    [SerializeField] private float blueMultiplier = 0.3f;
    
    [Header("Number of cubes")]
    [SerializeField] private int redNumber;
    [SerializeField] private int greenNumber;
    [SerializeField] private int blueNumber;

    [Header("Distance from ceiling")]
    [SerializeField] private float distanceFromCeiling;
    
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
    }

    private void Update()
    {
        //Remove this when tests are done
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
    /// <para>Calculates the distance from the ceiling</para>
    /// </summary>
    private void UpdateScalePosition()
    {
        float stretchAmount = redNumber * redMultiplier + greenNumber * greenMultiplier + blueNumber * blueMultiplier;
        
        //To set a constant speed for DOMove, no matter what the distance is, we need these calculations //TYT FİZİK
        float targetPositionY = weightlessPosition.y - stretchAmount;
        float distance = math.abs(transform.position.y - targetPositionY);
        float duration = distance / moveSpeed;
        
        transform.DOMoveY(targetPositionY, duration).SetEase(Ease.Linear);

        //Normally, we should use transform.position.y instead of targetPositionY, but DOMoveY is a coroutine
        //It's not finished by the end of this method. Eventually transform.position.y will be targetPositionY
        distanceFromCeiling = fixedPosition.y - targetPositionY;
    }

    private static void CheckCompletion()
    {
        
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