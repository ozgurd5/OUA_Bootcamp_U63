using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharController : MonoBehaviour
{
    public float moveSpeed = 5f;        // Character movement speed
    public float jumpForce = 5f;        // Jump force
    public Transform groundCheck;       // Ground check object
    public LayerMask groundMask;        // Layer mask for the ground

    public Transform cameraTransform;   // Reference to the camera transform
    public float mouseSensitivity = 2f; // Mouse sensitivity for camera movement

    private Rigidbody rb;
    private bool isGrounded;
    private float cameraRotation = 0f;
    private float characterRotation = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Check if the character is grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, groundMask);

        // Character movement
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        rb.velocity = new Vector3(move.x * moveSpeed, rb.velocity.y, move.z * moveSpeed);

        // Jumping
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        // Camera rotation
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        cameraRotation -= mouseY;
        cameraRotation = Mathf.Clamp(cameraRotation, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(cameraRotation, 0f, 0f);

        characterRotation += mouseX;
        transform.rotation = Quaternion.Euler(0f, characterRotation, 0f);
    }
}


