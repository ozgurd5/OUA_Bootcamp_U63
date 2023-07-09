using Unity.Netcode;
using UnityEngine;

/// <summary>
/// <para>Responsible for player animation</para>
/// <para>Works for each player</para>
/// </summary>
public class PlayerAnimationManager : NetworkBehaviour
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
        HandleEasterEgg();

        if (psd.currentMainState != PlayerStateData.PlayerMainState.NormalState) return;
        
        if (psd.isIdle)
            an.Play("PlayerIdle");
        else if (psd.isWalking)
            an.Play("PlayerWalking");
        else if (psd.isRunning)
            an.Play("PlayerRunning");
    }

    /// <summary>
    /// <para>Handles easter egg state's animation</para>
    /// </summary>
    private void HandleEasterEgg()
    {
        if (psd.currentMainState == PlayerStateData.PlayerMainState.EasterEggState) an.Play("EasterEgg");
    }
}
