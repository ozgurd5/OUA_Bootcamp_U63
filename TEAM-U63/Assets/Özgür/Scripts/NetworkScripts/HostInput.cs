using Unity.Netcode;
using UnityEngine;

//HOST SIDE

public class HostInput : NetworkBehaviour
{
    public static float horizontalInput;
    public static float verticalInput;
    private PlayerControls pc;
    
    private void Start()
    {
        pc = new PlayerControls();
        pc.Enable();
        pc.PlayerMovement.Enable();
    }
    
    private void Update()
    {
        if (!IsHost) return;
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        Debug.Log(pc.PlayerMovement.Movement.ReadValue<Vector2>());
        Debug.Log("space: " + pc.PlayerMovement.Jump.WasPressedThisFrame());
    }
}