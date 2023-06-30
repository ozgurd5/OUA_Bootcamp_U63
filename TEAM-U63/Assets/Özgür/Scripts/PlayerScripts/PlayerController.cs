using Unity.Netcode;
using UnityEngine;

//TODO: remove test only line in awake before build

/// <summary>
/// <para>Controls player movement, rotation, jump</para>
/// <para>Works only in "Normal State"</para>
/// </summary>
public class PlayerController : NetworkBehaviour
{
    [Header("IMPORTANT - SELECT OR NOT")]
    [SerializeField] private bool isCoder;

    [Header("Assign")]
    [SerializeField] private float walkingSpeed = 6f;
    [SerializeField] private float runningSpeed = 10f;
    [SerializeField] private float jumpSpeed = 5f;
    [SerializeField] private float rotatingSpeed = 5f;

    private NetworkPlayerData npd;
    private NetworkInputManager nim;
    private NetworkInputManager.InputData input;
    private PlayerStateData psd;

    private Transform cameraTransform;
    private Rigidbody rb;

    private Vector3 lookingDirectionForward;
    private Vector3 lookingDirectionRight;
    private Vector3 movingDirection;
    private float movingSpeed = 10f;    //Default moving speed is walking speed

    private bool jumpCondition;
    private float jumpBufferLimit = 0.2f;
    private float jumpBufferTimer;

    private void Start()
    {
        //TEST ONLY
        NetworkManager.Singleton.StartHost();
        //TEST ONLY

        npd = NetworkPlayerData.Singleton;
        nim = NetworkInputManager.Singleton;
        psd = GetComponent<PlayerStateData>();
        
        rb = GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform;

        DecideForInputSource();
        npd.OnIsHostCoderChanged += obj => DecideForInputSource();
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
    /// <para>Calculates looking direction</para>
    /// <para>Must work in FixedUpdate, idk why but Update stutters</para>
    /// </summary>
    private void CalculateLookingDirection()
    {
        lookingDirectionForward = cameraTransform.forward;
        lookingDirectionForward.y = 0f;

        lookingDirectionRight = cameraTransform.right;
        lookingDirectionRight.y = 0f;
    }
    
    /// <summary>
    /// <para>Calculates moving direction</para>
    /// <para>Should work in FixedUpdate</para>
    /// </summary>
    private void CalculateMovingDirection()
    {
        movingDirection = lookingDirectionRight * input.moveInput.x + lookingDirectionForward * input.moveInput.y;
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
    /// <para>Switches between idle and moving via rigidbody velocity</para>
    /// <para>Must work in Update since it must work before DecideWalkingOrRunningStates</para>
    /// </summary>
    private void DecideIdleOrMovingStates()
    {
        psd.isMoving = rb.velocity.magnitude > 0.01f;   //It's never 0f, idk why
        psd.isIdle = !psd.isMoving;
    }
    
    /// <summary>
    /// <para>Switches between walking and running state via input, sets speed according to it</para>
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
            movingSpeed = runningSpeed;
        else
            movingSpeed = walkingSpeed;
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
    
    /// <summary>
    /// <para>Handles jump buffer time for smooth gameplay</para>
    /// <para>When user press the space, player has jumpBufferLimit time to touch the ground.</para>
    /// <para>For example when player is falling down, user doesn't have to press the jump button in the perfect time
    /// to jump right again. User press the button when player is in the air, if player touch ground within the
    /// buffer time, it jumps</para>
    /// <para>Must work in Update since it has input check</para>
    /// </summary>
    private void HandleJumpBufferTime()
    {
        if (input.isJumpKeyDown)
            jumpBufferTimer = jumpBufferLimit;
        else
            jumpBufferTimer -= Time.deltaTime;
    }
    
    /// <summary>
    /// <para>Checks if conditions for jump are set</para>
    /// <para>Should work in Update</para>
    /// </summary>
    private void CheckJumpCondition()
    {
        if (jumpBufferTimer > 0f && psd.isGrounded)
            jumpCondition = true;
    }
    
    /// <summary>
    /// <para>Makes the player jump if conditions are set</para>
    /// <para>Must work in FixedUpdate</para>
    /// </summary>
    private void HandleJump()
    {
        if (jumpCondition)
        {
            rb.velocity += new Vector3(0f, jumpSpeed, 0f);
            psd.isJumping = true;
            
            //Must reset these variables to ensure that HandleJump is called once, not repeatedly
            jumpCondition = false;
            jumpBufferTimer = 0f;
        }
    }

    private void Update()
    {
        if (psd.currentState != PlayerStateData.PlayerState.NormalState) return;

        DecideIdleOrMovingStates();
        DecideWalkingOrRunningStates();

        HandleJumpBufferTime();
        CheckJumpCondition();
    }

    private void FixedUpdate()
    {
        if (psd.currentState != PlayerStateData.PlayerState.NormalState) return;
        
        CalculateLookingDirection();
        CalculateMovingDirection();
        SyncLookingDirection();
        
        HandleMovement();
        HandleJump();
    }
}