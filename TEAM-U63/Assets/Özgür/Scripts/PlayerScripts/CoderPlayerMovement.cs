using Unity.Netcode;
using UnityEngine;

public class CoderPlayerMovement : NetworkBehaviour
{
    [SerializeField] private float speed = 12f;
    
    private float horizontalInput;
    private float verticalInput;
    
    private Rigidbody rb;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    private void Update()
    {
        if (PlayerSelection.isHostCoder)
        {
            horizontalInput = HostInput.horizontalInput;
            verticalInput = HostInput.verticalInput;
        }
        
        else
        {
            horizontalInput = ClientInput.horizontalInput;
            verticalInput = ClientInput.verticalInput;
        }
        
        rb.velocity = new Vector3(horizontalInput * speed, rb.velocity.y, verticalInput * speed);
    }
}
