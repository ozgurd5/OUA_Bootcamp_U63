using UnityEngine;
using UnityEngine.UI;

public class PlayerGrab : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private Image crosshairImage;
    [SerializeField] private float grabRange = 5f;
    
    //1
    [SerializeField] private Transform holdArea;
    private GameObject heldObj;
    private Rigidbody heldObjRB;
    [SerializeField] private float pickupForce = 150.0f;
    //1

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

        if (Physics.Raycast(crosshairRay, out crosshairHit, grabRange) && Input.GetKeyDown(KeyCode.E))
        {
            //2
            if (crosshairHit.collider.CompareTag("RedPuzzle") || crosshairHit.collider.CompareTag("GreenPuzzle") || crosshairHit.collider.CompareTag("BluePuzzle"))
            {
                PickUpObject(crosshairHit.collider.gameObject);
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && heldObj != null)
        {
            DropObject();
        }
        
        if (heldObj != null)
            MoveObject();
        //2
    }
    
    //3
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
        heldObj = pickObj;
        heldObjRB = pickObj.GetComponent<Rigidbody>();

        heldObj.GetComponent<IsGrabbed>().isGrabbed = true;

        heldObj.transform.parent = holdArea;
        
        heldObjRB.useGravity = false;
        heldObjRB.drag = 10;
        heldObjRB.constraints = RigidbodyConstraints.FreezeRotation;
    }
    
    void DropObject()
    {
        heldObjRB.useGravity = true; 
        heldObjRB.drag = 1;
        heldObjRB.constraints = RigidbodyConstraints.None;
        
        heldObj.GetComponent<IsGrabbed>().isGrabbed = false;
        
        heldObjRB.transform.parent = null;
        heldObj = null;
        
    }
    //3
}
