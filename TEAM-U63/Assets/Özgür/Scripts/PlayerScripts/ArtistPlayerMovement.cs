using Unity.Netcode;
using UnityEngine;

//SERVER SIDE

//This script takes input from client and controls artist in server side

public class ArtistPlayerMovement : NetworkBehaviour
{
    [SerializeField] private float speed = 12f;
    
    public float horizontalInput;
    public float verticalInput;
    
    private Rigidbody rb;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    private void Update()
    {
        //Comment for client authoritative control and comment ClientInput.cs
        if (!IsHost) return;
        rb.velocity = new Vector3(horizontalInput * speed, rb.velocity.y, verticalInput * speed);
        
        //Uncomment for client authoritative control
        // if (IsHost) return;
        // rb.velocity = new Vector3(Input.GetAxisRaw("Horizontal") * speed, rb.velocity.y, Input.GetAxisRaw("Vertical") * speed);
        // SendServerRpc(transform.position.x, transform.position.z);
    }
    
    //Uncomment for client authoritative control
    // [ServerRpc(RequireOwnership = false)]
    // private void SendServerRpc(float x, float z)
    // {
    //     transform.position = new Vector3(x, transform.position.y, z);
    // }
}