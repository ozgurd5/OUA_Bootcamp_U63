using Unity.Netcode;
using UnityEngine;

/// <summary>
/// <para>Works in client side. Takes input from client side and sends it to the host side, so host can perfor actions</para>
/// </summary>
public class ClientInput : NetworkBehaviour
{
    public static float horizontalInput;
    public static float verticalInput;
    public static bool isRotateKey;
    
    private void Update()
    {
        if (IsHost) return;
        
        SendClientInputServerRpc(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), Input.GetKey(KeyCode.R));
    }

    /// <summary>
    /// <para>Takes input from client side and sends it to the host side, so host can perform actions</para>
    /// </summary>
    /// <param name="horizontal">Horizontal input</param>
    /// <param name="vertical">Vertical input</param>
    [ServerRpc(RequireOwnership = false)]
    private void SendClientInputServerRpc(float horizontal, float vertical, bool rotateKey)
    {
        horizontalInput = horizontal;
        verticalInput = vertical;
        isRotateKey = rotateKey;
    }
}