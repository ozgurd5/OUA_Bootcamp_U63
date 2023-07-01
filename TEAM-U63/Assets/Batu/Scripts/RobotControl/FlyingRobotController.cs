using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class FlyingRobotController : MonoBehaviour, RobotInputManager.IRobotControlActions
{
    [SerializeField] private float movementSpeed = 8f;
    [SerializeField] private float liftSpeed = 4f;

    [SerializeField] private float rotationSpeed = 4f; // Added rotationSpeed variable


    private RobotInputManager robotInputManager;
    private Rigidbody rb;
    private Transform characterTransform;
    private Transform cameraTransform;

    private Vector3 lookingDirectionForward;
    private Vector3 lookingDirectionRight;

    private Vector2 movementInput;
    private float ascendInput;
    private float descendInput;

    private void Awake()
    {
        robotInputManager = new RobotInputManager();
        rb = GetComponent<Rigidbody>();
        characterTransform = transform;

        cameraTransform = Camera.main.transform;

    }

    private void OnEnable()
    {
        robotInputManager.RobotControl.Enable();
        robotInputManager.RobotControl.SetCallbacks(this);
    }

    private void OnDisable()
    {
        robotInputManager.RobotControl.Disable();
        robotInputManager.RobotControl.RemoveCallbacks(this);
    }

    private void Update()
    {
        lookingDirectionForward = cameraTransform.forward;
        lookingDirectionForward.y = 0f;

        lookingDirectionRight = cameraTransform.right;
        lookingDirectionRight.y = 0f;

    }

    private void FixedUpdate()
    {
        Vector3 movement = movementInput.x * lookingDirectionRight + lookingDirectionForward * movementInput.y;
        rb.velocity = movement * movementSpeed;

        characterTransform.forward = Vector3.Slerp(cameraTransform.forward, lookingDirectionForward, Time.fixedDeltaTime * rotationSpeed);

        rb.velocity += new Vector3(0f, (ascendInput - descendInput) * liftSpeed, 0f);
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    public void OnDescend(InputAction.CallbackContext context)
    {
        descendInput = context.ReadValue<float>();
    }

    public void OnAscend(InputAction.CallbackContext context)
    {
        ascendInput = context.ReadValue<float>();
    }

    public void OnThirdPersonLook(InputAction.CallbackContext context)
    {
        // Implement the logic for handling the third-person look input here
        // For example, you can read the input value and rotate the robot accordingly
    }
}
