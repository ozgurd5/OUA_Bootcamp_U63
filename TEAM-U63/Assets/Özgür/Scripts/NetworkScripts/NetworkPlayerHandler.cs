using Unity.Netcode;
using UnityEngine;

public class NetworkPlayerHandler : NetworkBehaviour
{
    private ArtistPlayerMovement apm;
    private Rigidbody rb;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        apm = GetComponent<ArtistPlayerMovement>();
        rb.isKinematic = false;
    }
    
    private void Update()
    {
        if (!IsHost) return;
        rb.velocity = new Vector3(apm.horizontalMove.Value * 10, rb.velocity.y, apm.verticalMove.Value * 12);
    }
}
