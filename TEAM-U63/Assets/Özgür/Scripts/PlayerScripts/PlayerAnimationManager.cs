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
        if (psd.currentState == PlayerStateData.PlayerState.EasterEggState)
            an.Play("EasterEgg");
        
        if (psd.currentState != PlayerStateData.PlayerState.NormalState) return;
        
        //isJumping and 'isWalking/isRunning' can be true at the same time. Player can move and jump.
        //When that happens, jump animation must play, not the walking or running.
        if (psd.isJumping)
        {
            if (psd.isMoving)
                an.Play("PlayerMovingJumping");
            else
                an.Play("PlayerNotMovingJumping");
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
