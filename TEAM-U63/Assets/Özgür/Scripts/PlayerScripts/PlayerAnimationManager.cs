using Unity.Netcode;
using UnityEngine;

/// <summary>
/// <para>Responsible for player animation</para>
/// </summary>
public class PlayerAnimationManager : NetworkBehaviour
{
    private PlayerStateData psd;
    private Animator an;
    
    //
    private LazyNetworkSendHostAnimation lazy;

    public enum Animation
    {
        Idle,
        Walking,
        Running,
        EasterEgg
    }

    private Animation currentAnimation;
    
    private void Awake()
    {
        psd = GetComponent<PlayerStateData>();
        an = GetComponent<Animator>();
        
        //
        lazy = GetComponent<LazyNetworkSendHostAnimation>();
    }

    private void Update()
    {
        HandleEasterEgg();
        
        if (IsHost)
        {
            if (psd.currentState != PlayerStateData.PlayerState.NormalState) return;
        
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

        //
        else
        {
            if (psd.currentState != PlayerStateData.PlayerState.NormalState) return;
        
            if (lazy.networkIsIdle && currentAnimation != Animation.Idle)
            {
                an.Play("PlayerIdle");
                currentAnimation = Animation.Idle;
            }
        
            else if (lazy.networkIsWalking && currentAnimation != Animation.Walking)
            {
                an.Play("PlayerWalking");
                currentAnimation = Animation.Walking;
            }
        
            else if (lazy.networkIsRunning && currentAnimation != Animation.Running)
            {
                an.Play("PlayerRunning");
                currentAnimation = Animation.Running;
            }
        }
    }

    /// <summary>
    /// <para>Handles easter egg state's animation</para>
    /// </summary>
    private void HandleEasterEgg()
    {
        if (psd.currentState != PlayerStateData.PlayerState.EasterEggState) return;
        if (currentAnimation == Animation.EasterEgg) return;
        
        an.Play("EasterEgg");
        currentAnimation = Animation.EasterEgg;
    }
}
