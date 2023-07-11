using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class FlyingRobotController : MonoBehaviour, RobotInputManager.IRobotControlActions
{
    [SerializeField] private float movementSpeed = 8f;
    [SerializeField] private float liftSpeed = 4f;
    [SerializeField] private float rotationSpeed = 4f; 
    
    private RobotInputManager _robotInputManager;
    private Rigidbody _rb;
    private Transform _characterTransform;
    private Transform _cameraTransform;

    private Vector3 _lookingDirectionForward;
    private Vector3 _lookingDirectionRight;

    private Vector2 _movementInput;
    private float _ascendInput;
    private float _descendInput;

    private void Awake()
    {
        _robotInputManager = new RobotInputManager();
        _rb = GetComponent<Rigidbody>();
        _characterTransform = transform;

        _cameraTransform = Camera.main.transform;

    }

    private void OnEnable()
    {
        _robotInputManager.RobotControl.Enable();
        _robotInputManager.RobotControl.SetCallbacks(this);
    }

    private void OnDisable()
    {
        _robotInputManager.RobotControl.Disable();
        _robotInputManager.RobotControl.RemoveCallbacks(this);
    }

    private void Update()
    {
        _lookingDirectionForward = _cameraTransform.forward;
        _lookingDirectionForward.y = 0f;

        _lookingDirectionRight = _cameraTransform.right;
        _lookingDirectionRight.y = 0f;

    }

    private void FixedUpdate()
    {
        Vector3 movement = _movementInput.x * _lookingDirectionRight + _lookingDirectionForward * _movementInput.y;
        _rb.velocity = movement * movementSpeed;

        _characterTransform.forward = Vector3.Slerp(_cameraTransform.forward, _lookingDirectionForward, Time.fixedDeltaTime * rotationSpeed);

        _rb.velocity += new Vector3(0f, (_ascendInput - _descendInput) * liftSpeed, 0f);
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        _movementInput = context.ReadValue<Vector2>();
    }

    public void OnDescend(InputAction.CallbackContext context)
    {
        _descendInput = context.ReadValue<float>();
    }

    public void OnAscend(InputAction.CallbackContext context)
    {
        _ascendInput = context.ReadValue<float>();
    }

    public void OnThirdPersonLook(InputAction.CallbackContext context)
    {
        
    }
}
