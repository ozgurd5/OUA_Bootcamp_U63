using UnityEngine;

public class TPSCamera : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -5f);
    
    private Transform followTargetTransform;
    private Transform lookAtTargetTransform;
    private PlayerInputManager pim;

    private Vector3 followTargetPreviousPosition;
    private Vector3 followTargetPositionDifference;
    
    private void Start()
    {
        //TODO: that wont work on robots, fix it
        FindComponents();
        
        //Default values
        transform.position = followTargetTransform.position + offset;
        followTargetPreviousPosition = followTargetTransform.position;
    }

    //TODO: that wont work on robots, fix it
    private void FindComponents()
    {
        if (name == "CoderCamera")
        {
            followTargetTransform = GameObject.Find("CoderPlayer").transform;
            lookAtTargetTransform = followTargetTransform.Find("CoderCameraLookAt");
        }

        else
        {
            followTargetTransform = GameObject.Find("ArtistPlayer").transform;
            lookAtTargetTransform = followTargetTransform.Find("ArtistCameraLookAt");
        }
        
        pim = followTargetTransform.GetComponent<PlayerInputManager>();
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