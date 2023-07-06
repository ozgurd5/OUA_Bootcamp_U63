using UnityEngine;
using UnityEngine.UI;

public class PlayerGrabbing : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private Image crosshairImage;
    [SerializeField] private float grabRange = 5f;
    [SerializeField] private float movingForce = 150f;
    [SerializeField] private Transform grabPoint;

    private PlayerStateData psd;
    private PlayerController pc;
    private Camera cam;
    
    private Ray crosshairRay;
    private RaycastHit crosshairHit;
    
    private GameObject grabbedCube;
    private CubeStateManager grabbedCubeStateManager;
    private Rigidbody grabbedCubeRb;

    private void Awake()
    {
        psd = GetComponent<PlayerStateData>();
        pc = GetComponent<PlayerController>();
        cam = Camera.main;
    }

    /// <summary>
    /// <para>Casts ray for cubes</para>
    /// </summary>
    /// <returns>True if a ray hits a cube</returns>
    private bool CastRayForCubes()
    {
        crosshairRay = cam.ScreenPointToRay(crosshairImage.rectTransform.position);
        bool wasRayHit = Physics.Raycast(crosshairRay, out crosshairHit, grabRange);
        
        if (!wasRayHit) return false;
        
        Collider col = crosshairHit.collider; //Shorter return statement :p
        return col.CompareTag("RedPuzzle") || col.CompareTag("GreenPuzzle") || col.CompareTag("BluePuzzle");
    }
    
    /// <summary>
    /// <para>Picks the object up</para>
    /// <para>Must work in Update but it's conditions must be given in the Update to avoid conflicts with DropObject</para>
    /// </summary>
    private void PickUpObject()
    {
        grabbedCube = crosshairHit.collider.gameObject;
        grabbedCubeRb = grabbedCube.GetComponent<Rigidbody>();
        
        grabbedCubeStateManager = grabbedCube.GetComponent<CubeStateManager>();
        grabbedCubeStateManager.isGrabbed = true;

        //These values prevent all kind of stuttering, flickering, shaking, lagging etc.
        grabbedCubeRb.useGravity = false;
        grabbedCubeRb.drag = 10f;
        grabbedCubeRb.constraints = RigidbodyConstraints.FreezeRotation;
        
        //Parenting is needed for smooth movement and good looking motion
        grabbedCube.transform.parent = grabPoint.transform;
        
        psd.isGrabbing = true;
    }
    
    /// <summary>
    /// <para>Drops the object and basically do opposite of what PickUpObject method do</para>
    /// <para>Must work in Update and it's conditions must be given in the Update to avoid conflicts with PickUpObject</para>
    /// </summary>
    private void DropObject()
    {
        psd.isGrabbing = false;

        grabbedCube.transform.parent = null;
        
        grabbedCubeRb.useGravity = true;
        grabbedCubeRb.drag = 0f;
        grabbedCubeRb.constraints = RigidbodyConstraints.None;
        
        grabbedCubeStateManager.isGrabbed = false;
        grabbedCubeStateManager = null;
        
        grabbedCube = null;
        grabbedCubeRb = null;
    }
    
    /// <summary>
    /// <para>Moves the object</para>
    /// <para>Must work in FixedUpdate</para>
    /// </summary>
    private void MoveObject()
    {
        if (!psd.isGrabbing) return;
        
        //Cube must not follow grabPoint's position all the time, if it does it won't be in the..
        //..perfect position and therefore stutter when it should be stop moving
        if (Vector3.Distance(grabPoint.position, grabbedCube.transform.position) > 0.1f)
        {
            Vector3 moveDirection = (grabPoint.position -  grabbedCube.transform.position).normalized;
            grabbedCubeRb.AddForce(moveDirection * movingForce);
        }
    }

    private void Update()
    {
        //Input and isGrabbing condition check must be checked here instead of inside of the methods
        //If not, it's get broken. Maybe there is a way to make safer methods with it's condition checks are..
        //..inside of it, idk, this is the best i can
        
        if (!pc.input.isGrabKeyDown) return;

        if (psd.isGrabbing)
            DropObject();
        
        else if (CastRayForCubes())
            PickUpObject();
    }

    private void FixedUpdate()
    {
        MoveObject();
    }
}
