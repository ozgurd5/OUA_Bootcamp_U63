using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPickerUpper : MonoBehaviour
{
    public float pickupDistance = 3f;  // Maximum pickup distance
    public LayerMask pickupMask;      // Layer mask for pickupable objects

    private Transform cameraTransform; // Reference to the camera transform
    private GameObject pickedObject;   // Currently picked up object
    private bool isCarryingObject;     // Flag indicating if an object is being carried
    private Rigidbody objectRigidbody; // Reference to the picked object's rigidbody

    private void Start()
    {
        // Get the main camera transform
        cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        if (!isCarryingObject)
        {
            // Check if an object can be picked up
            if (Input.GetButtonDown("Fire1"))
            {
                Ray ray = cameraTransform.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, pickupDistance, pickupMask))
                {
                    // An object was hit by the raycast, pick it up
                    pickedObject = hit.collider.gameObject;
                    objectRigidbody = pickedObject.GetComponent<Rigidbody>();

                    // Disable physics simulation for the picked object
                    objectRigidbody.isKinematic = true;
                    objectRigidbody.detectCollisions = false;

                    // Set the object's parent to the interaction transform (e.g., character's hand)
                    pickedObject.transform.SetParent(cameraTransform);

                    isCarryingObject = true;
                    Debug.Log("Interacting with object: " + gameObject.name);
                }
            }
        }
        else
        {
            // Check if the carried object should be released
            if (Input.GetButtonDown("Fire1"))
            {
                // Release the object
                pickedObject.transform.SetParent(null);

                // Enable physics simulation and collision detection for the released object
                objectRigidbody.isKinematic = false;
                objectRigidbody.detectCollisions = true;

                pickedObject = null;
                objectRigidbody = null;
                isCarryingObject = false;
            }
        }
    }
}
