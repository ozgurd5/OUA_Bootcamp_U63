using System;
using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;

/// <summary>
/// <para>Controls player movement, rotation, jump</para>
/// <para>Works only in "Normal State"</para>
/// </summary>
public class PlayerController : NetworkBehaviour
{
    [Header("IMPORTANT - SELECT OR NOT")]
    public bool isCoder;

    [Header("Assign")]
    [SerializeField] private float walkingSpeed = 3f;
    [SerializeField] private float runningSpeed = 10f;
    [SerializeField] private float walkingRotatingSpeed = 5f;
    [SerializeField] private float runningRotatingSpeed = 20f;

    private NetworkInputManager nim;
    public NetworkInputManager.InputData input;
    private PlayerStateData psd;
    private Rigidbody rb;

    private Vector3 movingDirection;
    private float movingSpeed;
    private float rotatingSpeed;

    public event Action OnEasterEggEnter;
    public event Action OnEasterEggExit;

    private void Awake()
    {
        //Moving and rotating speed defaults must be walking speeds
        movingSpeed = walkingSpeed;
        rotatingSpeed = walkingRotatingSpeed;
        
        nim = NetworkInputManager.Singleton;
        psd = GetComponent<PlayerStateData>();
        
        rb = GetComponent<Rigidbody>();
        
        //TODO: remove before build, not necessary
        DecideForInputSource();
    }
    
    /// <summary>
    /// <para>Gets coder or artist input as the input source</para>
    /// </summary>
    private void DecideForInputSource()
    {
        if (isCoder)
            input = nim.coderInput;
        else
            input = nim.artistInput;
    }
    
    /// <summary>
    /// <para>Calculates moving direction</para>
    /// <para>Should work in FixedUpdate</para>
    /// </summary>
    private void CalculateMovingDirection()
    {
        Vector3 lookingDirectionRight = Vector3.Cross(Vector3.up, input.lookingDirection);
        
        movingDirection = lookingDirectionRight * input.moveInput.x + input.lookingDirection * input.moveInput.y;
        movingDirection.y = 0f;
    }
    
    /// <summary>
    /// <para>Turns the player to looking direction</para>
    /// <para>Must work in FixedUpdate, because Update stutters, idk why</para>
    /// </summary>
    private void SyncLookingDirection()
    {
        if (!psd.isMoving) return;
        
        transform.forward = Vector3.Slerp(transform.forward, movingDirection, rotatingSpeed * Time.fixedDeltaTime);
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
        
        psd.isRunning = input.isRunKey;
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
        //We have to constantly call this method due to reference changes in NetworkInputManager's ServerRpc method
        //Detailed explanation is in there
        DecideForInputSource();
        
        HandleEasterEgg();
        
        if (psd.currentState != PlayerStateData.PlayerState.NormalState) return;

        DecideIdleOrMovingStates();
        DecideWalkingOrRunningStates();
    }

    private void FixedUpdate()
    {
        if (psd.currentState != PlayerStateData.PlayerState.NormalState) return;
        
        CalculateMovingDirection();
        SyncLookingDirection();
        
        HandleMovement();
    }
    
    /// <summary>
    /// <para>Invokes events that controls entering and exiting easter egg state</para>
    /// <para>Don't and must not work in ability state</para>
    /// <para>Must work in Update since it has input check</para>
    /// </summary>
    private void HandleEasterEgg()
    {
        if (psd.currentState == PlayerStateData.PlayerState.AbilityState) return;
        
        if (input.isEasterEggKeyDown)
            OnEasterEggEnter?.Invoke();
        else if (input.isEasterEggKeyUp)
            OnEasterEggExit?.Invoke();
    }
}