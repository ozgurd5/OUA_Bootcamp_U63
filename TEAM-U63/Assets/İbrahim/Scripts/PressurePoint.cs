using UnityEngine;

public class PressurePoint : MonoBehaviour
{
    [SerializeField] private GameObject targetObject;
    [SerializeField] private string activationTag = "Player";

    private Animator animator;
    
    private bool isPuzzlePlaced;

    private void Start()
    {
        animator = targetObject.GetComponent<Animator>();
        animator.SetBool("IsPuzzlePlaced", false);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(activationTag) && !isPuzzlePlaced)
        {
            animator.SetBool("IsPuzzlePlaced", true);
            isPuzzlePlaced = true;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(activationTag)) return;
        
        animator.SetBool("IsPuzzlePlaced", false);
        isPuzzlePlaced = false;
    }
}