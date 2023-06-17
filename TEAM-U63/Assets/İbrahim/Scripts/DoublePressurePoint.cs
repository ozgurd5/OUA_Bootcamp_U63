using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoublePressurePoint : MonoBehaviour
{
    public GameObject puzzlePiece;
    public GameObject player;
    public GameObject targetObject;
    public string animationName;

    private Animator animator;
    [SerializeField] private static bool isPuzzlePlaced = false;
    [SerializeField] private static int PlacedObjects;

    private void Start()
    {
        // Disable the Animator component initially
        animator = targetObject.GetComponent<Animator>();
        
    }

    private void Update()
    {
        if (PlacedObjects == 2)
        {
            
            // Set the bool parameter in the Animator controller
            animator.SetBool("IsPuzzlePlaced", true);
        }
        else
        {
            
            animator.SetBool("IsPuzzlePlaced", false);
        }
        
        
        Debug.Log(PlacedObjects);
        Debug.Log(isPuzzlePlaced);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("puzzlePiece") || other.CompareTag("Player"))
        {
            

            // Play the animation by name
            //animator.Play(animationName);

            

            // Set the puzzle as placed to prevent repeated triggering
            PlacedObjects++;
            
            
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("puzzlePiece") || other.CompareTag("Player"))
        {
            // Reset the bool parameter in the Animator controller
            animator.SetBool("IsPuzzlePlaced", false);
            

           

            // Set the puzzle as not placed to allow triggering again
            PlacedObjects--;
        }
    }
}
