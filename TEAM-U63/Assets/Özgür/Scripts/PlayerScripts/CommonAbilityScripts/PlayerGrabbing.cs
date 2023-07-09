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
    [SerializeField] private float grabRange = 5f;
    [SerializeField] private float movingForce = 150f;
    [SerializeField] private Transform grabPoint;

    private PlayerData pd;
    private PlayerInputManager pim;
    private PlayerStateData psd;
    private Camera cam;

    private Ray crosshairRay;
    private RaycastHit crosshairHit;
    
    private GameObject grabbedCube;
    private CubeManager grabbedCubeManager;
    private Rigidbody grabbedCubeRb;

    private void Awake()
    {
        pd = GetComponent<PlayerData>();
        pim = GetComponent<PlayerInputManager>();
        psd = GetComponent<PlayerStateData>();
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
    /// <para>Must work in Update and it's conditions must be given in the Update to avoid conflicts with DropObject</para>
    /// </summary>
    private void PickUpObject()
    {
        grabbedCube = crosshairHit.collider.gameObject;
        grabbedCubeRb = grabbedCube.GetComponent<Rigidbody>();
        
        grabbedCubeManager = grabbedCube.GetComponent<CubeManager>();
        grabbedCubeManager.isGrabbed = true;

        //These values prevent all kind of stuttering, flickering, shaking, lagging etc.
        grabbedCubeRb.useGravity = false;
        grabbedCubeRb.drag = 15f;
        grabbedCubeRb.constraints = RigidbodyConstraints.FreezeRotation;
        
        //Parenting is needed for smooth movement and good looking motion
        //grabbedCube.transform.parent = grabPoint.transform;
        
        psd.isGrabbing = true;
    }
    
    /// <summary>
    /// <para>Drops the object and basically do opposite of what PickUpObject method does</para>
    /// <para>Must work in Update and it's conditions must be given in the Update to avoid conflicts with PickUpObject</para>
    /// </summary>
    private void DropObject()
    { 
        psd.isGrabbing = false;

        //grabbedCube.transform.parent = null;
        
        grabbedCubeRb.useGravity = true;
        grabbedCubeRb.drag = 0f;
        grabbedCubeRb.constraints = RigidbodyConstraints.None;
        
        grabbedCubeManager.isGrabbed = false;
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
        
        grabbedCube.transform.position = grabPoint.transform.position;
        
        //Cube must not follow grabPoint's position all the time, if it does it won't be in the..
        //..perfect position and therefore stutter when it should be stop moving
        //if (Vector3.Distance(grabPoint.position, grabbedCube.transform.position) > 0.1f)
        //{
        //    Vector3 moveDirection = (grabPoint.position - grabbedCube.transform.position).normalized;
        //    grabbedCubeRb.AddForce(moveDirection * movingForce);
        //}
    }

    private void Update()
    {
        if (!pim.isGrabKeyDown) return;

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