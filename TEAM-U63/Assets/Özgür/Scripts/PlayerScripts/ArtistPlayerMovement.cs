using Unity.Netcode;
using UnityEngine;

//SERVER SIDE

//This script takes input from client and controls artist in server side

public class ArtistPlayerMovement : NetworkBehaviour
{
    [SerializeField] private float speed = 12f;
    public float interpolationValue = 0.5f;
    
    public float horizontalInput;
    public float verticalInput;
    
    private Rigidbody rb;
    private Vector3 smoothDampVelocity;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    private void Update()
    {
        if (!IsHost) return;
        
        rb.velocity = new Vector3(horizontalInput * speed, rb.velocity.y, verticalInput * speed);
        ClientInterpolationClientRPC(transform.position);
    }
    
    [ClientRpc]
    private void ClientInterpolationClientRPC(Vector3 targetPosition)
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, interpolationValue);
    }
}