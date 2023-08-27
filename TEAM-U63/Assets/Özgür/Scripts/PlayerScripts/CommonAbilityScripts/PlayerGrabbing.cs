using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// <para>Responsible of grabbing, dropping and moving with the cube</para>
/// </summary>
public class PlayerGrabbing : NetworkBehaviour
{
    [Header("ASSIGN - BROKEN")]
    [SerializeField] private CrosshairManager cm;
    
    [Header("Assign - NetworkParentListID")]
    [SerializeField] private int grabPointNetworkParentListID;

    [Header("Assign - Audio Source")]
    [SerializeField] private AudioSource aus;
    
    [Header("Assign")]
    [SerializeField] private float walkingMovingForce = 150f;
    [SerializeField] private float runningMovingForce = 300f;
    [SerializeField] private float bufferDistance = 0.1f;
    [SerializeField] private float cubeDrag = 15f;

    private PlayerInputManager pim;
    private PlayerStateData psd;
    
    private Transform grabPointTransform;
    private GameObject grabbedCube;
    private CubeManager grabbedCubeManager;
    private Rigidbody grabbedCubeRb;

    private float movingForce;

    private void Awake()
    {
        pim = GetComponent<PlayerInputManager>();
        psd = GetComponent<PlayerStateData>();
        //cm = GetComponentInChildren<CrosshairManager>();
        grabPointTransform = Camera.main!.transform.Find("GrabPoint"); 
    }
    
    private void PickUpObject()
    {
        aus.Play();
        
        grabbedCube = cm.crosshairHit.collider.gameObject;
        grabbedCubeRb = grabbedCube.GetComponent<Rigidbody>();
        grabbedCubeManager = grabbedCube.GetComponent<CubeManager>();
        
        //Prevent other player's pick up when the cube is already picked
        if (grabbedCubeManager.isGrabbed) return;
        
        grabbedCubeManager.UpdateIsGrabbed(true);
        grabbedCubeManager.ChangeCubeLocalStatus(true);

        //These settings prevent all kind of stuttering, flickering, shaking, lagging etc.
        grabbedCubeManager.UpdateGravity(false);
        grabbedCubeRb.drag = cubeDrag;
        grabbedCubeRb.constraints = RigidbodyConstraints.FreezeRotation;

        grabbedCubeManager.UpdateParentUsingNetworkParentListID(grabPointNetworkParentListID);
        psd.isGrabbing = true;
    }
    
    private void DropObject()
    {
        aus.Play();
        
        psd.isGrabbing = false;
        grabbedCubeManager.UpdateParentUsingNetworkParentListID(-1);    //-1 means null
        
        grabbedCubeRb.constraints = RigidbodyConstraints.None;
        grabbedCubeRb.drag = 0f;
        grabbedCubeManager.UpdateGravity(true);
        
        grabbedCubeManager.UpdateIsGrabbed(false);
        
        grabbedCubeManager = null;
        grabbedCubeRb = null;
        grabbedCube = null;
    }

    //Must work in FixedUpdate
    private void MoveObject()
    {
        if (!psd.isGrabbing) return;

        if (psd.isRunning) movingForce = runningMovingForce;
        else movingForce = walkingMovingForce;
        
        //Cube must not follow grab point's position all the time, if it does it won't be in the..
        //..perfect position and therefore stutter when it should be stop moving
        if (Vector3.Distance(grabPointTransform.position, grabbedCube.transform.position) > bufferDistance)
        {
            //It's not a direction actually because we are not normalizing the vector. We must not, because..
            //..the cube should move faster if the distance is greater. If we normalize the distance, cube moves..
            //..at the same speed whatever the distance is
            Vector3 moveDirection = grabPointTransform.position - grabbedCube.transform.position;
            grabbedCubeRb.AddForce(moveDirection * movingForce);
        }
    }

    private void Update()
    {
        if (pim.isGrabKeyDown)
        {
            if (psd.isGrabbing) DropObject();
            else if (cm.isLookingAtCube) PickUpObject();
        }
    }

    private void FixedUpdate()
    {
        MoveObject();
    }
}