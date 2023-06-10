using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteraction : MonoBehaviour
{
    public float interactDistance = 3f; // Maximum interaction distance
    public LayerMask interactMask; // Layer mask for interactable objects

    private Transform cameraTransform; // Reference to the camera transform

    private void Start()
    {
        // Get the main camera transform
        cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        // Interact with objects
        if (Input.GetButtonDown("Fire1"))
        {
            Ray ray = cameraTransform.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, interactDistance, interactMask))
            {
                // An object was hit by the raycast, you can perform actions here
                InteractableObject interactableObject = hit.collider.GetComponent<InteractableObject>();
                if (interactableObject != null)
                {
                    interactableObject.Interact();
                }
            }
        }
    }
}
