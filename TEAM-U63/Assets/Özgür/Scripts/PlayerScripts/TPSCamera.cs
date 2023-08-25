using UnityEngine;

public class TPSCamera : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private Transform followTargetTransform;
    [SerializeField] private Transform lookAtTargetTransform;
    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -5f);
    
    private Vector3 followTargetPreviousPosition;
    private Vector3 followTargetPositionDifference;

    private void Start()
    {
        transform.position = followTargetTransform.position + offset;
        
        //Default value
        followTargetPreviousPosition = followTargetTransform.position;
    }

    private void LateUpdate()
    {
        followTargetPositionDifference = followTargetTransform.position - followTargetPreviousPosition;

        transform.position += followTargetPositionDifference;
        transform.LookAt(lookAtTargetTransform);
        
        followTargetPreviousPosition = followTargetTransform.position;
    }
}