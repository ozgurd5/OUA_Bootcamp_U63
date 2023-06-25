using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FlyingRobotController : MonoBehaviour, RobotInputManager.IRobotControlActions
{
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float ascendSpeed = 1f;
    [SerializeField] private float descendSpeed = 1f;

    private RobotInputManager robotInputManager;
    private Rigidbody rb;

    private Vector2 movementInput;
    private float ascendInput;
    private float descendInput;

    private void Awake()
    {
        robotInputManager = new RobotInputManager();
        rb = GetComponent<Rigidbody>();
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
        Vector3 movement = new Vector3(movementInput.x, ascendInput - descendInput, movementInput.y) * movementSpeed;
        rb.velocity = movement;
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    public void OnDescend(InputAction.CallbackContext context)
    {
        descendInput = context.ReadValue<float>() * descendSpeed;
    }

    public void OnAscend(InputAction.CallbackContext context)
    {
        ascendInput = context.ReadValue<float>() * ascendSpeed;
    }
}
