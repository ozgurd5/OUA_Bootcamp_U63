using Unity.Netcode;
using UnityEngine;

/// <summary>
/// <para>Takes input from the host</para>
/// </summary>
public class HostInput : NetworkBehaviour
{
    public static float horizontalInput;
    public static float verticalInput;
    public static bool isRotateKey;

    private void Update()
    {
        if (!IsHost) return;
        
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        isRotateKey = Input.GetKey(KeyCode.R);
    }
}