using UnityEngine;

public class RobotController : MonoBehaviour
{
    [Header("Assign")] [SerializeField] private float movingSpeed = 8f;
    [SerializeField] private float verticalSpeed = 4f;
    [SerializeField] private float rotatingSpeed = 0.4f;

    private RobotManager rm;
    private PlayerInputManager pim;
    private Rigidbody rb;
    private Transform cameraTransform;

    private Vector3 movingDirection;
    private float verticalDirection;

    private void Awake()
    {
        rm = GetComponent<RobotManager>();
        pim = GetComponent<PlayerInputManager>();
        rb = GetComponent<Rigidbody>();
        cameraTransform = GameObject.Find("RobotCamera").transform;
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
