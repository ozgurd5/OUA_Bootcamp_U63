using UnityEngine;

/// <summary>
/// <para>Responsible for player animation</para>
/// </summary>
public class PlayerAnimationManager : MonoBehaviour
{
    private PlayerStateData psd;
    private Animator an;

    public enum Animation
    {
        Idle,
        MovingJumping,
        NotMovingJumping,
        Walking,
        Running,
        EasterEgg
    }

    private Animation currentAnimation;
    
    private void Awake()
    {
        psd = GetComponent<PlayerStateData>();
        an = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleEasterEgg();
        
        if (psd.currentState != PlayerStateData.PlayerState.NormalState) return;
        
        //isJumping and 'isWalking/isRunning' can be true at the same time. Player can move and jump.
        //When that happens, jump animation must play, not the walking or running.
        if (psd.isJumping)
        {
            if (psd.isMoving && currentAnimation != Animation.MovingJumping && currentAnimation != Animation.NotMovingJumping)
            {
                an.Play("PlayerMovingJumping");
                currentAnimation = Animation.MovingJumping;
            }
            
            else if (currentAnimation != Animation.MovingJumping && currentAnimation != Animation.NotMovingJumping)
            {
                an.Play("PlayerNotMovingJumping");
                currentAnimation = Animation.NotMovingJumping;
            }
            
            return;
        }
        
        //After this point, only one state can be true at the same time, so no problem.
        if (psd.isIdle && currentAnimation != Animation.Idle)
        {
            an.Play("PlayerIdle");
            currentAnimation = Animation.Idle;
        }
        
        else if (psd.isWalking && currentAnimation != Animation.Walking)
        {
            an.Play("PlayerWalking");
            currentAnimation = Animation.Walking;
        }
        
        else if (psd.isRunning && currentAnimation != Animation.Running)
        {
            an.Play("PlayerRunning");
            currentAnimation = Animation.Running;
        }
    }

    private void HandleEasterEgg()
    {
        if (psd.currentState == PlayerStateData.PlayerState.EasterEggState && currentAnimation != Animation.EasterEgg)
        {
            an.Play("EasterEgg");
            currentAnimation = Animation.EasterEgg;
        }
    }
}
