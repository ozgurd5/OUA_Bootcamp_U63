using Unity.Netcode;
using UnityEngine;

public class NetworkInputManager : NetworkBehaviour
{
    public NetworkInputActions nia;
    
    public struct InputData : INetworkSerializable
    {
        public Vector2 moveInput;
        public bool isRotatingKey;
        public bool isJumpKeyDown;
        
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref moveInput);
            serializer.SerializeValue(ref isRotatingKey);
            serializer.SerializeValue(ref isJumpKeyDown);
        }
    }
    
    public static InputData hostInput;
    public static InputData clientInput;
    
    private void Awake()
    {
        nia = new NetworkInputActions();
        nia.Player.Enable();
    }

    private void Update()
    {
        if (IsHost)
        {
            hostInput.moveInput = nia.Player.Move.ReadValue<Vector2>();
            hostInput.isRotatingKey = nia.Player.Rotate.IsPressed();
            hostInput.isJumpKeyDown = nia.Player.Jump.WasPressedThisFrame();
        }
        
        else
        {
            clientInput.moveInput = nia.Player.Move.ReadValue<Vector2>();
            clientInput.isRotatingKey = nia.Player.Rotate.IsPressed();
            clientInput.isJumpKeyDown = nia.Player.Jump.WasPressedThisFrame();
            SendClientInputServerRpc(clientInput);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SendClientInputServerRpc(InputData clientInputInClientSide)
    {
        clientInput = clientInputInClientSide;
    }
}