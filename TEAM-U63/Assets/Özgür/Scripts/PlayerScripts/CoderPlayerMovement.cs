using Unity.Netcode;
using UnityEngine;

public class CoderPlayerMovement : NetworkBehaviour
{
    [SerializeField] private float speed = 12f;

    private NetworkInputManager nim;
    private Rigidbody rb;

    private void Start()
    {
        nim = NetworkInputManager.Singleton;
        rb = GetComponent<Rigidbody>();
    }
    
    private void Update()
    {
        if (!IsHost) return;
        rb.velocity = new Vector3(nim.coderInput.moveInput.x * speed, rb.velocity.y, nim.coderInput.moveInput.y * speed);
        
        if (nim.coderInput.isRotatingKey)
        {
            transform.Rotate(0f, 360f * Time.deltaTime, 0f);
        }
    }
}
