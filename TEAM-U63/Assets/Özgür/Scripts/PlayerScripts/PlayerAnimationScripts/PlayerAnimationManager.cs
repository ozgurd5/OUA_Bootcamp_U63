using Unity.Netcode;
using UnityEngine;

/// <summary>
/// <para>Responsible for player animation</para>
/// <para>Works for each player</para>
/// </summary>
public class PlayerAnimationManager : NetworkBehaviour
{
    private PlayerStateData psd;
    private PlayerController pc;
    private Animator an;

    private float blendValue;

    private void Awake()
    {
        psd = GetComponent<PlayerStateData>();
        pc = GetComponent<PlayerController>();
        an = GetComponent<Animator>();
    }
    
    private void Update()
    {
        an.SetFloat("PlayerWalkAndRunBlendValue", MovingSpeedToBlendValue(pc.movingSpeed));
        
        if (psd.currentMainState == PlayerStateData.PlayerMainState.EasterEggState) an.Play("EasterEgg");
        else if (psd.currentMainState is PlayerStateData.PlayerMainState.AbilityState or PlayerStateData.PlayerMainState.RobotControllingState) an.Play("PlayerIdle");
        else if (psd.isIdle) an.Play("PlayerIdle");
        else if (psd.isMoving) an.Play("PlayerWalkAndRun");
    }

    private float MovingSpeedToBlendValue(float movingSpeed)
    {
        if (movingSpeed < 3f) return 0f;
        else return (movingSpeed - pc.walkingSpeed) / (pc.runningSpeed - pc.walkingSpeed);
    }
}
