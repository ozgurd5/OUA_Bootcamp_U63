using Unity.Netcode;
using UnityEngine;

public class CoderPlayerMovement : NetworkBehaviour
{
    [SerializeField] private float speed = 12f;
    
    private NetworkInputManager.InputData input;
    
    private Rigidbody rb;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        input = NetworkInputManager.coderInput;
    }
    
    private void Update()
    {
        if (!IsHost) return;

        rb.velocity = new Vector3(input.moveInput.x * speed, rb.velocity.y, input.moveInput.y * speed);

        if (input.isRotatingKey)
        {
            transform.Rotate(0f, 360f * Time.deltaTime, 0f);
        }
    }
}
