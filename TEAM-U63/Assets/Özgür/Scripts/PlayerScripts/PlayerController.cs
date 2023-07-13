using System;
using Cinemachine;
using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;

/// <summary>
/// <para>Controls player movement, rotation, jump</para>
/// <para>Works only for local player</para>
/// </summary>
public class PlayerController : NetworkBehaviour
{
    [Header("Assign")]
    [SerializeField] private float walkingSpeed = 3f;
    [SerializeField] private float runningSpeed = 10f;
    [SerializeField] [Range(0,1)] private float walkingRotatingSpeed = 0.2f;
    [SerializeField] [Range(0,1)] private float runningRotatingSpeed = 0.4f;

    private PlayerData pd;
    private PlayerStateData psd;
    private PlayerInputManager pim;
    private Rigidbody rb;
    private CinemachineFreeLook cam;

    public float rotatingSpeed;
    private Vector3 movingDirection;
    private float movingSpeed;

    public event Action OnEasterEggEnter;
    public event Action OnEasterEggExit;

    private void Awake()
    {
        //Moving and rotating speed defaults must be walking speeds
        movingSpeed = walkingSpeed;
        rotatingSpeed = walkingRotatingSpeed;

        pd = GetComponent<PlayerData>();
        psd = GetComponent<PlayerStateData>();
        pim = GetComponent<PlayerInputManager>();
        rb = GetComponent<Rigidbody>();
        cam = GetComponentInChildren<CinemachineFreeLook>();
    }
    
    /// <summary>
    /// <para>Calculates moving direction</para>
    /// <para>Should work in FixedUpdate</para>
    /// </summary>
    private void CalculateMovingDirection()
    {
        Vector3 lookingDirectionRight = Vector3.Cross(Vector3.up, pim.lookingDirectionForward);
        
        movingDirection = lookingDirectionRight * pim.moveInput.x + pim.lookingDirectionForward * pim.moveInput.y;
        movingDirection.y = 0f;
    }
    
    /// <summary>
    /// <para>Turns the player to looking direction</para>
    /// <para>Must work in FixedUpdate, because Update stutters, idk why</para>
    /// </summary>
    private void TurnLookingDirection()
    {
        if (psd.isMoving) transform.forward = Vector3.Slerp(transform.forward, movingDirection, rotatingSpeed);
        else if (psd.isGrabbing && psd.isIdle) transform.forward = Vector3.Slerp(transform.forward, pim.lookingDirectionForward, rotatingSpeed);
    }

    /// <summary>
    /// <para>Switches between idle and moving via rigidbody velocity x and z</para>
    /// <para>Must work in Update since it must work before DecideWalkingOrRunningStates</para>
    /// </summary>
    private void DecideIdleOrMovingStates()
    {
        Vector2 velocityXZ = new Vector2(rb.velocity.x, rb.velocity.z);
        
        psd.isMoving = math.abs(velocityXZ.magnitude) > 0.01f;  //It's never 0, idk why
        psd.isIdle = !psd.isMoving;
    }
    
    /// <summary>
    /// <para>Switches between walking and running state via input, sets moving and rotating speeds according to it</para>
    /// <para>Must work Update since it has input check</para>
    /// </summary>
    private void DecideWalkingOrRunningStates()
    {
        if (!psd.isMoving)
        {
            psd.isWalking = false;
            psd.isRunning = false;
            return;
        }
        
        psd.isRunning = pim.isRunKey;
        psd.isWalking = !psd.isRunning;

        if (psd.isRunning)
        {
            movingSpeed = runningSpeed;
            rotatingSpeed = runningRotatingSpeed;
        }
        else
        {
            movingSpeed = walkingSpeed;
            rotatingSpeed = walkingRotatingSpeed;
        }
    }
    
    /// <summary>
    /// <para>Makes player move</para>
    /// <para>Must work in FixedUpdate</para>
    /// </summary>
    private void HandleMovement()
    {
        movingDirection *= movingSpeed;
        rb.velocity = new Vector3(movingDirection.x, rb.velocity.y, movingDirection.z);
    }

    private void Update()
    {
        if (!pd.isLocal) return;
        
        //TODO: easter egg
        //HandleEasterEgg();
        
        //Responsibility chart of the states/rigidbody/camera
        //1.a Robot - IsHacked Enter - PlayerQTEAbility.cs and RobotManager.cs
        //1.b Player - RobotControllingState Enter - PlayerQTEAbility.cs
        //2.a Robot - IsHacked Exit to IsSleeping - RobotManager.cs
        //2.b Player - RobotControllingState Exit to NormalState - PlayerController.cs
        
        //2.b
        if (pim.isPrimaryAbilityKeyDown && psd.currentMainState == PlayerStateData.PlayerMainState.RobotControllingState)
        {
            psd.currentMainState = PlayerStateData.PlayerMainState.NormalState;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            cam.enabled = true;
        }
        
        if (psd.currentMainState != PlayerStateData.PlayerMainState.NormalState) return;

        DecideIdleOrMovingStates();
        DecideWalkingOrRunningStates();
    }

    private void FixedUpdate()
    {
        if (!pd.isLocal) return;
        if (psd.currentMainState != PlayerStateData.PlayerMainState.NormalState) return;
        
        CalculateMovingDirection();
        TurnLookingDirection();
        
        HandleMovement();
    }
    
    /// <summary>
    /// <para>Invokes events that controls entering and exiting easter egg state</para>
    /// <para>Don't and must not work in ability state</para>
    /// <para>Must work in Update since it has input check</para>
    /// </summary>
    private void HandleEasterEgg()
    {
        if (psd.currentMainState == PlayerStateData.PlayerMainState.AbilityState) return;
        
        if (pim.isEasterEggKeyDown)
            OnEasterEggEnter?.Invoke();
        else if (pim.isEasterEggKeyUp)
            OnEasterEggExit?.Invoke();
    }
}