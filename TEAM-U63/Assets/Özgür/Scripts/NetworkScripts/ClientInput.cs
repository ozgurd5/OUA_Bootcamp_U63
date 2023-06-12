using Unity.Netcode;
using UnityEngine;

//CLIENT SIDE

public class ClientInput : NetworkBehaviour
{
    public static float horizontalInput;
    public static float verticalInput;
    
    private void Update()
    {
        if (IsHost) return;
        
        SendClientInputServerRpc(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }
    
    /// <summary>
    /// <para>Takes input from client side and sends it to the host side , so host can perform actions</para>
    /// </summary>
    /// <param name="horizontal">Horizontal input</param>
    /// <param name="vertical">Vertical input</param>
    [ServerRpc(RequireOwnership = false)]
    private void SendClientInputServerRpc(float horizontal, float vertical)
    {
        horizontalInput = horizontal;
        verticalInput = vertical;
    }
}