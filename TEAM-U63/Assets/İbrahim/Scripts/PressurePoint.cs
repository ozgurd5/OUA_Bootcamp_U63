using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePoint : MonoBehaviour
{
    public GameObject puzzlePiece;
    public GameObject targetObject;
    public string animationName;

    private Animator animator;
    //private Animation _animation;
    
    private bool isPuzzlePlaced = false;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == puzzlePiece && !isPuzzlePlaced)
        {
            // Place the puzzle piece on the pressure point
            // puzzlePiece.transform.position = transform.position;

            // Enable the Animator component on the target object
            animator = targetObject.GetComponent<Animator>();
            if (animator != null)
            {
                animator.enabled = true;
                animator.Play(animationName);

                // Set the bool parameter in the Animator controller
                animator.SetBool("IsPuzzlePlaced", true);

                // Set the puzzle as placed to prevent repeated triggering
                isPuzzlePlaced = true;
            }
        }
    }
    
    //private IEnumerator DisableAnimatorAfterDelay(float delay)
    //{
    //    yield return new WaitForSeconds(delay);
//
    //    //while (_animation.isPlaying.Equals(true))
    //    //{
    //    //    yield return null;
    //    //}
    //    
//
    //    // Disable the Animator component on the target object
    //    animator.enabled = false;
    //}

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == puzzlePiece)
        {
            // Reset the bool parameter in the Animator controller
            animator.SetBool("IsPuzzlePlaced", false);
            

            //float currentAnimationDuration = animator.GetCurrentAnimatorStateInfo(0).length;
            //
//
            //StartCoroutine(DisableAnimatorAfterDelay(currentAnimationDuration));
            
            

            // Set the puzzle as not placed to allow triggering again
            isPuzzlePlaced = false;
        }
    }
}

