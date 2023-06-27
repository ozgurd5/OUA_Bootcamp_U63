using System;
using Cinemachine;
using Unity.Netcode;
using UnityEngine;

//TODO: remove test only line in awake before build

public class PlayerController : NetworkBehaviour
{
    [Header("IMPORTANT - SELECT")]
    [SerializeField] private bool isCoder;

    [Header("Assign")]
    [SerializeField] private float walkingSpeed;
    [SerializeField] private float runningSpeed;

    private NetworkPlayerData npd;
    private NetworkInputManager nim;
    private NetworkInputManager.InputData input;
    private PlayerStateData psd;

    private CinemachineFreeLook cam;
    private Rigidbody rb;
    
    private Vector3 lookingDirectionForward;
    private Vector3 lookingDirectionRight;
    private Vector3 movingDirection;
    private float movingSpeed = 10f;

    private void Awake()
    {
        //TEST ONLY
        NetworkManager.Singleton.StartHost();
        //TEST ONLY

        npd = NetworkPlayerData.Singleton;
        nim = NetworkInputManager.Singleton;
        psd = GetComponent<PlayerStateData>();
        
        rb = GetComponent<Rigidbody>();
        cam = GetComponentInChildren<CinemachineFreeLook>();

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
    /// <para>Should be called in FixedUpdate</para>
    /// </summary>
    private void CalculateLookingDirections()
    {
        lookingDirectionForward = (transform.position - cam.transform.position).normalized;
        lookingDirectionForward.y = 0f;

        lookingDirectionRight = Vector3.Cross(Vector3.up, lookingDirectionForward).normalized;
        lookingDirectionRight.y = 0f;
    }
    
    /// <summary>
    /// <para>Turns the player to looking direction</para>
    /// </summary>
    private void SyncLookingDirection()
    {
        if (!psd.isMoving) return;
        
        transform.forward = lookingDirectionForward;
    }

    /// <summary>
    /// <para>Decide if the player is moving or not</para>
    /// <para>Must be decided before DecideWalkingOrRunningStates, so must be called in Update</para>
    /// </summary>
    private void DecideMovingState()
    {
        //It's never 0f, idk why
        psd.isMoving = rb.velocity.magnitude > 0.2f;
    }
    
    /// <summary>
    /// <para>Switches walking and running state via input, sets speed according to it</para>
    /// <para>Must be called in Update since it has input check</para>
    /// </summary>
    private void DecideWalkingOrRunningStates()
    {
        if (!psd.isMoving)
        {
            psd.isWalking = false;
            psd.isRunning = false;
            return;
        }
        
        if (input.isRunKeyDown)
            psd.isRunning = true;
        else if (input.isRunKeyUp)
            psd.isRunning = false;

        if (psd.isRunning)
            movingSpeed = runningSpeed;
        else
            movingSpeed = walkingSpeed;
    }

    private void Update()
    {
        DecideMovingState();
        DecideWalkingOrRunningStates();
    }

    private void FixedUpdate()
    {
        CalculateLookingDirections();
        SyncLookingDirection();
        movingDirection = lookingDirectionRight * input.moveInput.x + lookingDirectionForward * input.moveInput.y;
        rb.velocity = movingDirection * movingSpeed;
    }                                                        
}