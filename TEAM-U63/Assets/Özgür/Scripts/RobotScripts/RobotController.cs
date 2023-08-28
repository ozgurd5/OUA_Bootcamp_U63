using System.Collections;
using UnityEngine;

public class RobotController : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private float defaultMovingSpeed = 8f;
    [SerializeField] private float defaultVerticalSpeed = 4f;
    [SerializeField] private float acceleration = 20f;
    [SerializeField] private float rotatingSpeed = 0.4f;

    [Header("Info - No Touch")]
    [SerializeField] private bool isMoving;
    [SerializeField] private bool isVerticalMoving;
    [SerializeField] private bool isIncreasingMovingSpeed;
    [SerializeField] private bool isIncreasingVerticalSpeed;
    [SerializeField] private float movingSpeed;
    [SerializeField] private float verticalSpeed;
    [SerializeField] private Vector3 movingDirection;
    [SerializeField] private float verticalDirection;
    
    private RobotManager rm;
    private PlayerInputManager pim;
    private Rigidbody rb;
    private Transform cameraTransform;
    
    private IEnumerator increaseMovingSpeedCoroutine;
    private IEnumerator increaseVerticalSpeedCoroutine;

    private void Awake()
    {
        rm = GetComponent<RobotManager>();
        pim = GetComponent<PlayerInputManager>();
        rb = GetComponent<Rigidbody>();
        cameraTransform = GameObject.Find("RobotCamera").transform;

        increaseMovingSpeedCoroutine = IncreaseSpeed(true, defaultMovingSpeed);
        increaseVerticalSpeedCoroutine = IncreaseSpeed(false, defaultVerticalSpeed);
    }
    
    private void FixedUpdate()
    {
        if (rm.currentState != RobotManager.RobotState.Hacked) return;
        
        CalculateMovingAndVerticalDirection();
        HandleMovement();
    }

    private void Update()
    {
        if (rm.currentState != RobotManager.RobotState.Hacked) return;
        
        DecideForMovingStates();
        HandleMovingSpeed();
        HandleVerticalSpeed();
        TurnTowardsLookingDirection();
    }

    private void CalculateMovingAndVerticalDirection()
    {
        movingDirection = cameraTransform.right * pim.moveInput.x + cameraTransform.forward * pim.moveInput.y;
        movingDirection.y = 0f;

        if (pim.isAscendKey) verticalDirection = 1f;
        else if (pim.isDescendKey) verticalDirection = -1f;
        else verticalDirection = 0f;
    }

    private void TurnTowardsLookingDirection()
    {
        transform.forward = Vector3.Slerp(transform.forward, cameraTransform.forward, rotatingSpeed);
    }

    private void DecideForMovingStates()
    {
        isMoving = pim.moveInput != Vector2.zero;
        isVerticalMoving = pim.isAscendKey || pim.isDescendKey;
    }
    
    private void HandleMovingSpeed()
    {
        if (isMoving && !isIncreasingMovingSpeed)
        {
            StopCoroutine(increaseMovingSpeedCoroutine);
            isIncreasingMovingSpeed = false;

            increaseMovingSpeedCoroutine = IncreaseSpeed(true, defaultMovingSpeed);
            StartCoroutine(increaseMovingSpeedCoroutine);
        }

        else if (!isMoving)
        {
            StopCoroutine(increaseMovingSpeedCoroutine);
            isIncreasingMovingSpeed = false;

            movingSpeed = 0f;
        }
    }

    private void HandleVerticalSpeed()
    {
        if (isVerticalMoving && !isIncreasingVerticalSpeed)
        {
            StopCoroutine(increaseVerticalSpeedCoroutine);
            isIncreasingVerticalSpeed = false;

            increaseVerticalSpeedCoroutine = IncreaseSpeed(false, defaultVerticalSpeed);
            StartCoroutine(increaseVerticalSpeedCoroutine);
        }

        else if (!isVerticalMoving)
        {
            StopCoroutine(increaseVerticalSpeedCoroutine);
            isIncreasingVerticalSpeed = false;

            verticalSpeed = 0f;
        }
    }
    
    private IEnumerator IncreaseSpeed(bool isIncreasingMovingSeed, float speedToReach)
    {
        if (isIncreasingMovingSeed)
        {
            isIncreasingMovingSpeed = true;
            
            while (movingSpeed < speedToReach)
            {
                movingSpeed += acceleration * Time.deltaTime;
                yield return null;
            }

            if (movingSpeed > speedToReach) movingSpeed = speedToReach;
            
            isIncreasingMovingSpeed = false;
        }

        else
        {
            isIncreasingVerticalSpeed = true;
            
            while (verticalSpeed < speedToReach)
            {
                verticalSpeed += acceleration * Time.deltaTime;
                yield return null;
            }

            if (verticalSpeed > speedToReach) verticalSpeed = speedToReach;

            isIncreasingVerticalSpeed = false;
        }
    }

    private void HandleMovement()
    {
        //We have to make the moving directions magnitude equal to moving speed
        if (movingDirection.magnitude != 0f) //Prevents divide by zero error (which is not the case, it returns NaN)
        {
            float multiplier = movingSpeed / movingDirection.magnitude;
            movingDirection *= multiplier;
        }
        
        verticalDirection *= verticalSpeed;
        
        rb.velocity = new Vector3(movingDirection.x, verticalDirection, movingDirection.z);
    }
}
