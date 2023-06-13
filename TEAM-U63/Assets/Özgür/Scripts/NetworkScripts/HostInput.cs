using Unity.Netcode;
using UnityEngine;

//HOST SIDE

public class HostInput : NetworkBehaviour
{
    public static float horizontalInput;
    public static float verticalInput;

    private void Update()
    {
        if (!IsHost) return;
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }
}