using UnityEngine;

public class TPSCamera : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private Transform followTargetTransform;
    [SerializeField] private Transform lookAtTargetTransform;
    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -5f);
    
    [SerializeField] private PlayerInputManager pim;
    
    private Vector3 followTargetPreviousPosition;
    private Vector3 followTargetPositionDifference;
    
    private void Start()
    {
        //TODO: that wont work on robots, fix it
        pim = followTargetTransform.GetComponent<PlayerInputManager>();
        
        //Default values
        transform.position = followTargetTransform.position + offset;
        followTargetPreviousPosition = followTargetTransform.position;
    }

    private void Update()
    {
        transform.RotateAround(lookAtTargetTransform.position, transform.right, -pim.lookInput.y);
        transform.RotateAround(lookAtTargetTransform.position, Vector3.up, pim.lookInput.x);
    }

    private void LateUpdate()
    {
        followTargetPositionDifference = followTargetTransform.position - followTargetPreviousPosition;

        transform.position += followTargetPositionDifference;
        transform.LookAt(lookAtTargetTransform);
        
        followTargetPreviousPosition = followTargetTransform.position;
    }
}