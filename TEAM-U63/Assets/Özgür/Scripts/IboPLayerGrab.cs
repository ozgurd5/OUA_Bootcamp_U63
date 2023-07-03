using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attach to  Main Camera,
/// Create an empty Gameobject and name it HoldArea,
/// Attach HoldArea as child to Main Camera
/// </summary>

public class IboPLayerGrab : MonoBehaviour
{
    [SerializeField] private Transform holdArea;
    private GameObject heldObj;
    private Rigidbody heldObjRB;

    [SerializeField] private float pickupRange = 5.0f;
    [SerializeField] private float pickupForce = 150.0f;
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (heldObj == null)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit,
                        pickupRange))
                {
                    PickUpObject(hit.transform.gameObject);
                }
            }
            else
            {
                DropObject();
            }
        }

        if (heldObj != null)
        {
            MoveObject();
        }
    }

    void MoveObject()
    {
        if (Vector3.Distance(heldObj.transform.position, holdArea.position) > 0.1f)
        {
            Vector3 moveDirection = (holdArea.position - heldObj.transform.position);
            heldObjRB.AddForce(moveDirection * pickupForce);
        }
    }

    void PickUpObject(GameObject pickObj)
    {
        if (pickObj.GetComponent<Rigidbody>())
        {
            heldObjRB = pickObj.GetComponent<Rigidbody>();
            heldObjRB.useGravity = false;
            heldObjRB.drag = 10;
            heldObjRB.constraints = RigidbodyConstraints.FreezeRotation;

            heldObjRB.transform.parent = holdArea;
            heldObj = pickObj;
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
}
