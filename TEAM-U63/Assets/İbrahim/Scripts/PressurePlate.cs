using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] private GameObject targetObject;
    [SerializeField] private string activationTag = "Player";

    private Animator animator;
    private bool isPressed;

    private void Start()
    {
        animator = targetObject.GetComponent<Animator>();
        animator.SetBool("IsPuzzlePlaced", false);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(activationTag) && !isPressed)
        {
            animator.SetBool("IsPuzzlePlaced", true);
            isPressed = true;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(activationTag)) return;
        
        animator.SetBool("IsPuzzlePlaced", false);
        isPressed = false;
    }
}