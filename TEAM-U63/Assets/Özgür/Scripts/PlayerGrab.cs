using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGrab : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private Image crosshairImage;
    [SerializeField] private float grabRange = 5f;
    [SerializeField] private Transform holdArea;
    [SerializeField] private float pickupForce = 150.0f;    //

    private Camera cam;
    
    private GameObject grabbedObject;
    private Rigidbody grabbedObjectRb;
    private Ray crosshairRay;
    private RaycastHit crosshairHit;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        crosshairRay = cam.ScreenPointToRay(crosshairImage.rectTransform.position);
        
        if (Physics.Raycast(crosshairRay, out crosshairHit, grabRange) && Input.GetKeyDown(KeyCode.E))
        {
            if (crosshairHit.collider.CompareTag("RedPuzzle") || crosshairHit.collider.CompareTag("GreenPuzzle") || crosshairHit.collider.CompareTag("BluePuzzle"))
                PickUpObject(crosshairHit.collider.gameObject);
        }
        
        if (Input.GetKeyDown(KeyCode.R) && grabbedObject != null)
            DropObject();
        
        if (grabbedObject != null)
            MoveObjectToHoldArea();
    }
    
    void MoveObjectToHoldArea()
    {
        if (Vector3.Distance(grabbedObject.transform.position, holdArea.position) > 0.1f)
        {
            Vector3 moveDirection = holdArea.position - grabbedObject.transform.position;
            grabbedObjectRb.AddForce(moveDirection * pickupForce);
        }
        
        //grabbedObject.transform.DOMove(holdArea.transform.position, 1f)
    }
    
    void PickUpObject(GameObject pickObj)
    {
        grabbedObject = pickObj;
        grabbedObjectRb = pickObj.GetComponent<Rigidbody>();

        grabbedObject.GetComponent<CubeStateManager>().isGrabbed = true;

        //grabbedObject.transform.parent = holdArea;
        
        grabbedObjectRb.useGravity = false;
        grabbedObjectRb.drag = 10;
        grabbedObjectRb.constraints = RigidbodyConstraints.FreezeRotation;
    }
    
    void DropObject()
    {
        grabbedObjectRb.useGravity = true; 
        grabbedObjectRb.drag = 1;
        grabbedObjectRb.constraints = RigidbodyConstraints.None;
        
        grabbedObject.GetComponent<CubeStateManager>().isGrabbed = false;
        
        grabbedObject = null;
        
    }
}
