using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// <para>Responsible of grabbing, dropping and moving with the cube</para>
/// </summary>
public class PlayerGrabbing : NetworkBehaviour
{
    [Header("Assign")]
    [SerializeField] private Image crosshairImage;
    [SerializeField] private float grabRange = 7f;
    [SerializeField] private float walkingMovingForce = 150f;
    [SerializeField] private float runningMovingForce = 300f;
    [SerializeField] private Transform grabPointTransform;
    [SerializeField] private float bufferDistance = 0.1f;
    [SerializeField] private float cubeDrag = 15f;

    private PlayerData pd;
    private PlayerInputManager pim;
    private PlayerStateData psd;
    private CrosshairManager cm;

    private GameObject grabbedCube;
    private CubeManager grabbedCubeManager;
    private Rigidbody grabbedCubeRb;

    private float movingForce;

    private void Awake()
    {
        pd = GetComponent<PlayerData>();
        pim = GetComponent<PlayerInputManager>();
        psd = GetComponent<PlayerStateData>();
        cm = GetComponentInChildren<CrosshairManager>();
    }
    
    
    /// <summary>
    /// <para>Picks the object up</para>
    /// <para>Must work in Update</para>
    /// </summary>
    private void PickUpObject()
    {
        grabbedCube = cm.crosshairHit.collider.gameObject;
        grabbedCubeRb = grabbedCube.GetComponent<Rigidbody>();
        grabbedCubeManager = grabbedCube.GetComponent<CubeManager>();
        
        grabbedCubeManager.UpdateIsGrabbed(true);
        grabbedCubeManager.ChangeCubeLocalStatus(true);

        //These settings prevent all kind of stuttering, flickering, shaking, lagging etc.
        grabbedCubeManager.UpdateGravity(false);
        grabbedCubeRb.drag = cubeDrag;
        grabbedCubeRb.constraints = RigidbodyConstraints.FreezeRotation;

        psd.isGrabbing = true;
    }
     
    /// <summary>
    /// <para>Drops the object and basically do opposite of what PickUpObject method does</para>
    /// <para>Must work in Update</para>
    /// </summary>
    private void DropObject()
    {
        psd.isGrabbing = false;
        
        grabbedCubeRb.drag = 0f;
        grabbedCubeRb.constraints = RigidbodyConstraints.None;
        grabbedCubeManager.UpdateGravity(true);
        
        grabbedCubeManager.UpdateIsGrabbed(false);
        
        grabbedCubeManager = null;
        grabbedCube = null;
        grabbedCubeRb = null;
    }
    
    /// <summary>
    /// <para>Moves the object with the crosshair while holding the cube</para>
    /// <para>Must work in FixedUpdate</para>
    /// </summary>
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
        if (!pd.isLocal) return;
        if (!pim.isGrabKeyDown) return;

        if (psd.isGrabbing) DropObject();
        
        else if (cm.isLookingAtCube) PickUpObject();
    }

    private void FixedUpdate()
    {
        MoveObject();
    }
}