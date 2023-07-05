using UnityEngine;

public class PressurePoint : MonoBehaviour
{
    public GameObject puzzlePiece;
    public GameObject targetObject;

    private Animator animator;
    //private Animation _animation;
    
    private bool isPuzzlePlaced = false;

    private void Start()
    {
        animator = targetObject.GetComponent<Animator>();
        animator.SetBool("IsPuzzlePlaced", false);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == puzzlePiece && !isPuzzlePlaced)
        {
            // Enable the Animator component on the target object
            
            if (animator != null)
            {
                // Set the bool parameter in the Animator controller
                animator.SetBool("IsPuzzlePlaced", true);

                // Set the puzzle as placed to prevent repeated triggering
                isPuzzlePlaced = true;
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == puzzlePiece)
        {
            // Reset the bool parameter in the Animator controller
            animator.SetBool("IsPuzzlePlaced", false);
            
            isPuzzlePlaced = false;
        }
    }
}