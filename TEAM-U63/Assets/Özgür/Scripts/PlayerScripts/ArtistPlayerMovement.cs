using Unity.Netcode;
using UnityEngine;

public class ArtistPlayerMovement : NetworkBehaviour
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
        
        rb.velocity = new Vector3(ClientInput.horizontalInput * speed, rb.velocity.y, ClientInput.verticalInput * speed);
    }
}