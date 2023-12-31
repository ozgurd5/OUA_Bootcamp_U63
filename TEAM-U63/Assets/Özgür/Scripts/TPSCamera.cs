using Cinemachine;
using UnityEngine;

//TODO: CUSTOM CAMERA COLLISION: 8 STEP
public class TPSCamera : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -5f);
    
    //1
    //[Header("Debug")]
    //[SerializeField] private bool drawCameraSphere;
    //[SerializeField] private bool drawCameraRays;

    [Header("Info - No Touch")]
    [SerializeField] private CinemachineVirtualCamera cinemachineCam; //2
    [SerializeField] private Transform followTargetTransform;
    [SerializeField] private Transform lookAtTargetTransform;
    [SerializeField] private PlayerInputManager pim;
    [SerializeField] private bool canMoveUp;
    [SerializeField] private bool canMoveDown;

    private Vector3 followTargetPreviousPosition;
    private Vector3 followTargetPositionDifference;

    
    private void Start()
    {
        //3.1 //UpdateRobot in FindComponents need cinemachineCam
        cinemachineCam = GetComponent<CinemachineVirtualCamera>();
        
        FindComponents();
        
        //3.2 //lookAtTargetTransform is coming from FindComponents
        cinemachineCam.LookAt = lookAtTargetTransform;
        
        //Default values
        transform.position = followTargetTransform.position + offset;
        followTargetPreviousPosition = followTargetTransform.position;
    }
    
    private void FindComponents()
    {
        if (name == "CoderCamera")
        {
            followTargetTransform = GameObject.Find("CoderPlayer").transform;
            lookAtTargetTransform = followTargetTransform.Find("CoderCameraLookAt");
            pim = followTargetTransform.GetComponent<PlayerInputManager>();
        }

        else if (name == "ArtistCamera")
        {
            followTargetTransform = GameObject.Find("ArtistPlayer").transform;
            lookAtTargetTransform = followTargetTransform.Find("ArtistCameraLookAt");
            pim = followTargetTransform.GetComponent<PlayerInputManager>();
        }
        
        else if (name == "RobotCamera")
        {
            UpdateRobot();
            RobotManager.OnCurrentControlledRobotChanged += UpdateRobot;
        }
        
        else Debug.Log("NAME THE CAMERA CORRECTLY: CoderCamera - ArtistCamera - RobotCamera");
    }

    private void UpdateRobot()
    {
        followTargetTransform = RobotManager.currentControlledRobot.transform;
        lookAtTargetTransform = followTargetTransform.Find("RobotCameraLookAt");
        cinemachineCam.LookAt = lookAtTargetTransform;
        pim = followTargetTransform.GetComponent<PlayerInputManager>();
    }

    private void Update()
    {
        if (!cinemachineCam.enabled) return;
        
        transform.RotateAround(lookAtTargetTransform.position, Vector3.up, pim.lookInput.x);
        
        canMoveUp = transform.rotation.eulerAngles.x is < 85f or > 290f;
        canMoveDown = transform.rotation.eulerAngles.x is < 90f or > 305f;
        
        if ((canMoveDown && pim.lookInput.y > 0f) || canMoveUp && pim.lookInput.y < 0f)
            transform.RotateAround(lookAtTargetTransform.position, transform.right, -pim.lookInput.y);

        //4
        //CheckForCollision();
    }
    private void LateUpdate()
    {
        followTargetPositionDifference = followTargetTransform.position - followTargetPreviousPosition;
        transform.position += followTargetPositionDifference;
        
        //5
        //Cinemachine handles camera collision so it also must handle LookAt
        //transform.LookAt(lookAtTargetTransform);
        
        followTargetPreviousPosition = followTargetTransform.position;

        BugFix();
    }

    private void BugFix()
    {
        if (Input.GetKeyDown(KeyCode.B) && Input.GetKey(KeyCode.RightShift))
        {
            Vector3 temp = transform.position;
            temp.y = followTargetTransform.position.y + 5f;
            transform.position = temp;
        }
    }

    //6
    //public bool upHit;
    //public bool downHit;
    //public bool rightHit;
    //public bool leftHit;
    //public bool forwardHit;
    //public bool backHit;
    //public float cameraRadius = 0.2f;
    
    //7
    //private void CheckForCollision()
    //{
    //    upHit = Physics.Raycast(transform.position, transform.up, cameraRadius);
    //    downHit = Physics.Raycast(transform.position, -transform.up, cameraRadius);
    //    rightHit = Physics.Raycast(transform.position, transform.right, cameraRadius);
    //    leftHit = Physics.Raycast(transform.position, -transform.right, cameraRadius);
    //    forwardHit = Physics.Raycast(transform.position, transform.forward, cameraRadius);
    //    backHit = Physics.Raycast(transform.position, -transform.forward, cameraRadius);
    //}

    //8
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.magenta;
    //    
    //    if (drawCameraSphere) Gizmos.DrawWireSphere(transform.position, cameraRadius);
    //
    //    if (drawCameraRays)
    //    {
    //        Gizmos.DrawRay(transform.position, transform.up);
    //        Gizmos.DrawRay(transform.position, -transform.up);
    //        Gizmos.DrawRay(transform.position, transform.right);
    //        Gizmos.DrawRay(transform.position, -transform.right);
    //        Gizmos.DrawRay(transform.position, transform.forward);
    //        Gizmos.DrawRay(transform.position, -transform.forward);
    //    }
    //}

    private void OnDestroy()
    {
        if (name == "RobotCamera") RobotManager.OnCurrentControlledRobotChanged -= UpdateRobot;
    }
}