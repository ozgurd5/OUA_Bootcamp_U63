using Unity.Netcode;
using UnityEngine;

//CLIENT SIDE

//Takes input from client and sends it to server

public class ClientInput : NetworkBehaviour
{
    private ArtistPlayerMovement apm;
    
    private void Start()
    {
        apm = GetComponent<ArtistPlayerMovement>();
    }
    
    private void Update()
    {
        if (IsHost) return;
        SendClientInputServerRpc(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }
    
    [ServerRpc(RequireOwnership = false)]
    private void SendClientInputServerRpc(float horizontal, float vertical)
    {
        apm.horizontalInput = horizontal;
        apm.verticalInput = vertical;
    }
}