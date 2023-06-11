using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePoint : MonoBehaviour
{
    public GameObject puzzlePiece;
    public GameObject targetObject;
    public string animationName;

    private bool isPuzzlePlaced = false;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == puzzlePiece && !isPuzzlePlaced)
        {
            // Place the puzzle piece on the pressure point
            //puzzlePiece.transform.position = transform.position;

            // Enable the Animator component on the target object
            Animator animator = targetObject.GetComponent<Animator>();
            if (animator != null)
            {
                animator.enabled = true;
                animator.Play(animationName);
            }

            // Set the puzzle as placed to prevent repeated triggering
            isPuzzlePlaced = true;
        }
    }
}

