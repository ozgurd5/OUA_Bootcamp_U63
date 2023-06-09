using Unity.Netcode;
using UnityEngine;

public class ArtistPlayerMovement : NetworkBehaviour
{
    public NetworkVariable<float> horizontalMove;
    public NetworkVariable<float> verticalMove;
    
    private void Update()
    {
        if (IsHost) return;
        SendInputServerRpc(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    [ServerRpc(RequireOwnership = false)]
    private void SendInputServerRpc(float horizontal, float vertical)
    {
        horizontalMove.Value = horizontal;
        verticalMove.Value = vertical;
    }
}