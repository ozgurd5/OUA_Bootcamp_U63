using UnityEngine;

public class NPCTalk : MonoBehaviour
{
    public bool isTalking;
    public Animator animator;

    private void Start()
    {
        animator.SetBool("IsTalking", false); // Make sure the initial state is not talking
    }

    public void StartTalking()
    {
        if (!isTalking)
        {
            isTalking = true;
            animator.SetBool("IsTalking", true);
            animator.Play("TalkingAnimation", -1, 0); // Play the "TalkingAnimation" from the beginning
        }
    }

    public void StopTalking()
    {
        if (isTalking)
        {
            isTalking = false;
            animator.SetBool("IsTalking", false);
        }
    }
}