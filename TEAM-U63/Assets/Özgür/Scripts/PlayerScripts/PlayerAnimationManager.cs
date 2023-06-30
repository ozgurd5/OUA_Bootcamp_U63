using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    private PlayerStateData psd;
    private Animator an;

    private void Awake()
    {
        psd = GetComponent<PlayerStateData>();
        an = GetComponent<Animator>();
    }

    private void Update()
    {
        //isJumping and 'isWalking/isRunning' can be true at the same time. Player can move and jump.
        //When that happens, jump animation must play, not the walking or running.
        if (psd.isJumping)
        {
            an.Play("PlayerJumping");
            return;
        }
        
        if (Input.GetKey(KeyCode.O))
        {
            an.Play("SpinMeRightRound");
            return;
        }
        
        //After this point, only one state can be true at the same time, so no problem.
        if (psd.isIdle)
            an.Play("PlayerIdle");
        else if (psd.isWalking)
            an.Play("PlayerWalking");
        else if(psd.isRunning)
            an.Play("PlayerRunning");
    }
}
