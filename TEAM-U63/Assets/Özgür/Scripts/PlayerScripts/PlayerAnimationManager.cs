using Unity.Netcode;
using UnityEngine;

/// <summary>
/// <para>Responsible for player animation</para>
/// </summary>
public class PlayerAnimationManager : NetworkBehaviour
{
    private PlayerData pd;
    private PlayerStateData psd;
    private Animator an;

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
        pd = GetComponent<PlayerData>();
        psd = GetComponent<PlayerStateData>();
        an = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleEasterEgg();

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
