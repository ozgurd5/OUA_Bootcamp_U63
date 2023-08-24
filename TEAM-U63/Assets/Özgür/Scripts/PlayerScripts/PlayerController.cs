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

    private Transform cameraFollowTransform;
    private Transform cameraLookAtTransform;
    private float currentYRotation;
    private Vector3 defaultCameraFollowLocalPosition;

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

        cameraFollowTransform = transform.GetChild(2);
        cameraLookAtTransform = transform.GetChild(3);
        defaultCameraFollowLocalPosition = cameraFollowTransform.localPosition;
    }

    public bool flag;
    private void HandleLooking()
    {
        cameraFollowTransform.RotateAround(cameraLookAtTransform.position, cameraFollowTransform.right, -pim.lookInput.y);
        currentYRotation += pim.lookInput.x;
        
        if (psd.isMoving)
        {
            Vector3 temp = cameraFollowTransform.position;

            Vector3 rotationEuler = transform.localRotation.eulerAngles;
            transform.localRotation = Quaternion.Euler(new Vector3(rotationEuler.x, currentYRotation, rotationEuler.z));
            
            if (!flag)
            {
                cameraFollowTransform.position = temp;
                flag = true;
            }
        }

        else
        {
            cameraFollowTransform.RotateAround(cameraLookAtTransform.position, Vector3.up, pim.lookInput.x);
            flag = false;
        }
    }
    
    private void CalculateMovingDirection()
    {
        movingDirection = transform.right * pim.moveInput.x + transform.forward * pim.moveInput.y;
        movingDirection.y = 0f;
    }
    
    private void DecideIdleOrMovingStates()
    {
        Vector2 velocityXZ = new Vector2(rb.velocity.x, rb.velocity.z);
        
        psd.isMoving = math.abs(velocityXZ.magnitude) > 0.01f;  //It's never 0, idk why
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
        //1.a Robot - Hacked Enter - PlayerQTEAbility.cs and RobotManager.cs
        //1.b Player - RobotControllingState Enter - PlayerQTEAbility.cs
        //2.a Robot - Hacked Exit to Sleeping - RobotManager.cs
        //2.b Player - RobotControllingState Exit to NormalState - PlayerController.cs

        //2.b
        if (pim.isPrimaryAbilityKeyDown && psd.currentMainState == PlayerStateData.PlayerMainState.RobotControllingState)
        {
            psd.currentMainState = PlayerStateData.PlayerMainState.NormalState;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            //cam.enabled = true;
        }
        
        HandleLooking();
        DecideIdleOrMovingStates();
        DecideWalkingOrRunningStates();
    }

    private void FixedUpdate()
    {
        if (!pd.isLocal) return;
        if (psd.currentMainState != PlayerStateData.PlayerMainState.NormalState) return;
        
        CalculateMovingDirection();
        HandleMovement();
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