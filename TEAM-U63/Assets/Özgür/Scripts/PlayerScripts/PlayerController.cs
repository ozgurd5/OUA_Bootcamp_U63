using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

/// <summary>
/// <para>Controls player movement, rotation, jump</para>
/// <para>Works only for local player</para>
/// </summary>
public class PlayerController : NetworkBehaviour
{
    public event Action OnEasterEggEnter;
    public event Action OnEasterEggExit;
    public event Action OnMovementStarted;
    public event Action OnMovementStopped;
    public event Action OnRunningStarted;
    public event Action OnRunningStopped;
    
    [Header("Assign")]
    public float walkingSpeed = 3f;
    public float runningSpeed = 10f;
    [SerializeField] private float acceleration = 20f;
    [Range(0,1)] public float rotatingSpeed = 0.2f;
    
    [Header("Info - No Touch")]
    public float movingSpeed;
    [SerializeField] private Vector3 movingDirection;
    [SerializeField] private bool isIncreasingSpeed;
    [SerializeField] private bool isDecreasingSpeed;

    private PlayerData pd;
    private PlayerStateData psd;
    private PlayerInputManager pim;
    private PlayerCameraManager pcm;
    private Rigidbody rb;

    private void Awake()
    {
        //Moving and rotating speed defaults must be walking speeds
        movingSpeed = walkingSpeed;

        pd = GetComponent<PlayerData>();
        psd = GetComponent<PlayerStateData>();
        pim = GetComponent<PlayerInputManager>();
        pcm = GetComponent<PlayerCameraManager>();
        rb = GetComponent<Rigidbody>();
    }
    
    private void Update()
    {
        if (!pd.isLocal) return;

        //TODO: easter egg
        //HandleEasterEgg();
        
        //Responsibility chart of the states/rigidbody/camera
        //1.a Robot - Hacked Enter - PlayerQTEAbility.cs and RobotManager.cs
        //1.b Player - RobotControllingState Enter - PlayerQTEAbility.cs
        //2.a Robot - Hacked Exit to Sleeping - RobotManager.cs
        //2.b Player - RobotControllingState Exit to NormalState - PlayerController.cs

        //2.b
        if (pim.isPrimaryAbilityKeyDown && psd.currentMainState == PlayerStateData.PlayerMainState.RobotControllingState)
        {
            psd.currentMainState = PlayerStateData.PlayerMainState.NormalState;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            pcm.cam.enabled = true;
        }

        if (psd.currentMainState != PlayerStateData.PlayerMainState.NormalState) return;
        DecideIdleOrMovingStates();
        DecideWalkingOrRunningStates();
        HandleMovingSpeed();
    }

    private void FixedUpdate()
    {
        if (!pd.isLocal) return;
        if (psd.currentMainState != PlayerStateData.PlayerMainState.NormalState) return;
        
        CalculateMovingDirection();
        TurnTowardsMovingDirection();
        HandleMovement();
    }

    private void CalculateMovingDirection()
    {
        movingDirection = pcm.cameraTransform.right * pim.moveInput.x + pcm.cameraTransform.forward * pim.moveInput.y;
        movingDirection.y = 0f;
    }

    private void TurnTowardsMovingDirection()
    {
        if (psd.isMoving) transform.forward = Vector3.Slerp(transform.forward, movingDirection, rotatingSpeed);
        
        else if (psd.isGrabbing && psd.isIdle)
        {
            //To prevent rotation in x axis
            Vector3 target = pcm.cameraTransform.forward;
            target.y = 0f;
            
            transform.forward = Vector3.Slerp(transform.forward, target, rotatingSpeed);
        }
    }
    
    private void DecideIdleOrMovingStates()
    {
        bool previousIsMoving = psd.isMoving;
        
        psd.isMoving = pim.moveInput != Vector2.zero;
        psd.isIdle = !psd.isMoving;
        
        if (!previousIsMoving && psd.isMoving) OnMovementStarted?.Invoke();
        else if (previousIsMoving && !psd.isMoving) OnMovementStopped?.Invoke();
    }
    
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
    }
    
    private void HandleMovingSpeed()
    {
        if (psd.isIdle && movingSpeed != 0f && !isDecreasingSpeed) //Idle
        {
            StopSpeedCoroutines();
            StartCoroutine(DecreaseMovingSpeed(0f));
        }
        
        else if (psd.isWalking)
        {
            if (movingSpeed > walkingSpeed && !isDecreasingSpeed) //Running to walking
            {
                StopSpeedCoroutines();
                StartCoroutine(DecreaseMovingSpeed(walkingSpeed));
            }
            
            else if (movingSpeed != walkingSpeed && !isIncreasingSpeed && !isDecreasingSpeed) //Idle to walking
            {
                StopSpeedCoroutines();
                StartCoroutine(IncreaseMovingSpeed(walkingSpeed));
            }
        }
        
        else if (psd.isRunning && movingSpeed != runningSpeed && !isIncreasingSpeed) //Running
        {
            StopSpeedCoroutines();
            StartCoroutine(IncreaseMovingSpeed(runningSpeed));
        }
    }

    private void StopSpeedCoroutines()
    {
        StopAllCoroutines();
        isIncreasingSpeed = false;
        isDecreasingSpeed = false;
    }
    
    private IEnumerator IncreaseMovingSpeed(float movingSpeedToReach)
    {
        isIncreasingSpeed = true;
        
        while (movingSpeed < movingSpeedToReach)
        {
            movingSpeed += acceleration * Time.deltaTime;
            yield return null;
        }
        
        if (movingSpeed > movingSpeedToReach) movingSpeed = movingSpeedToReach;
        
        isIncreasingSpeed = false;
    }

    //Effects only animations. Input is 0 when the player stops, which makes the movingDirection 0. movingSpeed does..
    //not effect player movement what that happens. But since the animations are working according to movingSpeed, this..
    //..coroutine effect only animations
    private IEnumerator DecreaseMovingSpeed(float movingSpeedToReach)
    {
        isDecreasingSpeed = true;
        
        while (movingSpeed > movingSpeedToReach)
        {
            movingSpeed -= acceleration * Time.deltaTime;
            yield return null;
        }
        
        if (movingSpeed < movingSpeedToReach) movingSpeed = movingSpeedToReach;

        isDecreasingSpeed = false;
    }

    private void HandleMovement()
    {
        //We have to make the moving directions magnitude equal to moving speed
        if (movingDirection.magnitude != 0f) //Prevents divide by zero error (which is not the case, it returns NaN)
        {
            float multiplier = movingSpeed / movingDirection.magnitude;
            movingDirection *= multiplier;
        }

        rb.velocity = new Vector3(movingDirection.x, rb.velocity.y, movingDirection.z);
    }

    private void HandleEasterEgg()
    {
        if (psd.currentMainState == PlayerStateData.PlayerMainState.AbilityState) return;
        
        if (pim.isEasterEggKeyDown)
            OnEasterEggEnter?.Invoke();
        else if (pim.isEasterEggKeyUp)
            OnEasterEggExit?.Invoke();
    }
}