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
    
    [Header("Assign")]
    public float walkingSpeed = 3f;
    public float runningSpeed = 10f;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] [Range(0,1)] private float walkingRotatingSpeed = 0.2f;
    [SerializeField] [Range(0,1)] private float runningRotatingSpeed = 0.4f;
    
    [Header("Info - No Touch")]
    public float rotatingSpeed;
    public float movingSpeed;
    [SerializeField] private Vector3 movingDirection;
    [SerializeField] private bool isIncreasingSpeed;

    private PlayerData pd;
    private PlayerStateData psd;
    private PlayerInputManager pim;
    private PlayerCameraManager pcm;
    private Rigidbody rb;

    private void Awake()
    {
        //Moving and rotating speed defaults must be walking speeds
        movingSpeed = walkingSpeed;
        rotatingSpeed = walkingRotatingSpeed;

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
        psd.isMoving = pim.moveInput != Vector2.zero;
        psd.isIdle = !psd.isMoving;
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
        //TODO: fix repetitive code bellow:
        //StopAllCoroutines();
        //isIncreasingSpeed = false;
        
        if (psd.isIdle)
        {
            StopAllCoroutines();
            isIncreasingSpeed = false;
            
            movingSpeed = 0f;
        }
        
        else if (psd.isWalking)
        {
            if (movingSpeed > walkingSpeed) //From running to walking
            {
                StopAllCoroutines();
                isIncreasingSpeed = false;
                
                movingSpeed = walkingSpeed;
            }
            
            else if (!isIncreasingSpeed && movingSpeed != walkingSpeed)
            {
                StopAllCoroutines();
                isIncreasingSpeed = false;
                
                StartCoroutine(IncreaseMovingSpeed(walkingSpeed));
            }
        }
        
        else if (psd.isRunning && !isIncreasingSpeed && movingSpeed != runningSpeed)
        {
            StopAllCoroutines();
            isIncreasingSpeed = false;
            
            StartCoroutine(IncreaseMovingSpeed(runningSpeed));
        }
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