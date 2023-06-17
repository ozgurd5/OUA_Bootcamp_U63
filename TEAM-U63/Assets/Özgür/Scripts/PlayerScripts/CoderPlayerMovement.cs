using Unity.Netcode;
using UnityEngine;

//This line is the reason why we can use coderInput without referencing NetworkInputManager
using static NetworkInputManager;

public class CoderPlayerMovement : NetworkBehaviour
{
    [SerializeField] private float speed = 12f;

    private Rigidbody rb;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    private void Update()
    {
        if (!IsHost) return;
        rb.velocity = new Vector3(coderInput.moveInput.x * speed, rb.velocity.y, coderInput.moveInput.y * speed);
        
        if (coderInput.isRotatingKey)
        {
            transform.Rotate(0f, 360f * Time.deltaTime, 0f);
        }
    }
}
