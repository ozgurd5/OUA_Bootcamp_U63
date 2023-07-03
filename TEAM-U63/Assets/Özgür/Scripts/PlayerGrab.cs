using UnityEngine;
using UnityEngine.UI;

public class PlayerGrab : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private Image crosshairImage;
    [SerializeField] private float grabRange = 5f;
    
    //
    [SerializeField] private Transform holdArea;
    private GameObject heldObj;
    private Rigidbody heldObjRB;
    [SerializeField] private float pickupRange = 5.0f;
    [SerializeField] private float pickupForce = 150.0f;
    //

    private Camera cam;
    
    private Ray crosshairRay;
    private RaycastHit crosshairHit;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        crosshairRay = cam.ScreenPointToRay(crosshairImage.rectTransform.position);
        
        if (Physics.Raycast(crosshairRay, out crosshairHit, grabRange) && Input.GetMouseButtonDown(0))
        //
            PickUpObject(crosshairHit.collider.gameObject);

        if (Input.GetMouseButtonDown(1) && heldObj != null)
        {
            DropObject();
        }
        
        if (heldObj != null)
            MoveObject();
        //
    }
    
    //
    void MoveObject()
    {
        if (Vector3.Distance(heldObj.transform.position, holdArea.position) > 0.1f)
        {
            Vector3 moveDirection = holdArea.position - heldObj.transform.position;
            heldObjRB.AddForce(moveDirection * pickupForce);
        }
    }
    
    void PickUpObject(GameObject pickObj)
    {
        if (pickObj.GetComponent<Rigidbody>())
        {
            heldObj = pickObj;
            heldObjRB = pickObj.GetComponent<Rigidbody>();

            heldObj.transform.parent = holdArea;
            
            heldObjRB.useGravity = false;
            heldObjRB.drag = 10;
            heldObjRB.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }
    
    void DropObject()
    {
        heldObjRB.useGravity = true; 
        heldObjRB.drag = 1;
        heldObjRB.constraints = RigidbodyConstraints.None;
        
        heldObjRB.transform.parent = null;
        heldObj = null;
        
    }
    //
}
