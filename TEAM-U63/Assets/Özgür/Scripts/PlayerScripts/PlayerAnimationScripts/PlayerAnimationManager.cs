using System.Collections;
using Unity.Netcode;
using UnityEngine;

/// <summary>
/// <para>Responsible for player animation</para>
/// <para>Works for each player</para>
/// </summary>
public class PlayerAnimationManager : NetworkBehaviour
{
    [Header("Assign")]
    [SerializeField] private float idleAcceleration = 5f;
    [SerializeField] private float idleDeceleration = 5f;
    
    [Header("Info - No Touch")]
    public float idleAndMovingBlendValue;
    public float walkingAndRunningBlendValue;

    private PlayerData pd;
    private PlayerStateData psd;
    private PlayerController pc;
    private Animator an;

    private void Awake()
    {
        pd = GetComponent<PlayerData>();
        psd = GetComponent<PlayerStateData>();
        pc = GetComponent<PlayerController>();
        an = GetComponent<Animator>();

        pc.OnMovementStarted += MovePlayer;
        pc.OnMovementStopped += StopPlayer;
    }
    
    private void Update()
    {
        if (pd.isLocal) walkingAndRunningBlendValue = MovingSpeedToWalkAndRunBlendValue(pc.movingSpeed);
        
        an.SetFloat("PlayerIdleAndMovingBlendValue", idleAndMovingBlendValue);
        an.SetFloat("PlayerWalkAndRunBlendValue", walkingAndRunningBlendValue);
        
        if (psd.currentMainState == PlayerStateData.PlayerMainState.EasterEggState) an.Play("EasterEgg");
        else if (psd.currentMainState is PlayerStateData.PlayerMainState.AbilityState or PlayerStateData.PlayerMainState.RobotControllingState) an.Play("PlayerIdle");
        else an.Play("PlayerIdleAndMoving");
    }

    private float MovingSpeedToWalkAndRunBlendValue(float movingSpeed)
    {
        if (movingSpeed < 3f) return 0f;
        else return (movingSpeed - pc.walkingSpeed) / (pc.runningSpeed - pc.walkingSpeed);
    }

    private void MovePlayer()
    {
        StopAllCoroutines();
        StartCoroutine(IncreaseIdleAndMovingBlendValue());
    }

    private void StopPlayer()
    {
        StopAllCoroutines();
        StartCoroutine(DecreaseIdleAndMovingBlendValue());
    }
    
    private IEnumerator IncreaseIdleAndMovingBlendValue()
    {
        while (idleAndMovingBlendValue < 1f)
        {
            idleAndMovingBlendValue += idleAcceleration * Time.deltaTime;
            yield return null;
        }
        
        if (idleAndMovingBlendValue > 1f) idleAndMovingBlendValue = 1f;
    }
    
    private IEnumerator DecreaseIdleAndMovingBlendValue()
    {
        while (idleAndMovingBlendValue > 0f)
        {
            idleAndMovingBlendValue -= idleDeceleration * Time.deltaTime;
            yield return null;
        }
        
        if (idleAndMovingBlendValue < 0f) idleAndMovingBlendValue = 0f;
    }
}
