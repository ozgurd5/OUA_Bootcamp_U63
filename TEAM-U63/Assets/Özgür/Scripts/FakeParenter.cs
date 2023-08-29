using UnityEngine;

public class FakeParenter : MonoBehaviour
{
    [Header("Info - No Touch")]
    public Transform followTargetTransform;
    public bool isChild;
    
    private Vector3 followTargetPreviousPosition;
    private Vector3 followTargetPositionDifference;

    private void LateUpdate()
    {
        //Default value each time
        if (followTargetTransform != null && followTargetPreviousPosition == Vector3.zero)
            followTargetPreviousPosition = followTargetTransform.position;
        
        if (!isChild) return;
        
        followTargetPositionDifference = followTargetTransform.position - followTargetPreviousPosition;
        transform.position += followTargetPositionDifference;

        followTargetPreviousPosition = followTargetTransform.position;
    }
}
