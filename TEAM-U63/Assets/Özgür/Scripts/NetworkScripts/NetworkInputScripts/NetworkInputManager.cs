using Unity.Netcode;
using UnityEngine;

/// <summary>
/// <para>Gets host and client input. Decides for coder and artist input</para>
/// </summary>
public class NetworkInputManager : NetworkBehaviour
{
    private NetworkInputActions nia;
    
    /// <summary>
    /// <para>Holds input data</para>
    /// </summary>
    public class InputData : INetworkSerializable
    {
        public Vector2 moveInput;
        public bool isRotatingKey;
        public bool isJumpKeyDown;
        
        //We must teach Unity how to serialize InputData objects in order to transfer data in the network
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref moveInput);
            serializer.SerializeValue(ref isRotatingKey);
            serializer.SerializeValue(ref isJumpKeyDown);
        }
    }
    
    private static InputData hostInput;
    private static InputData clientInput;
    
    public static InputData coderInput;
    public static InputData artistInput;
    
    private void Awake()
    {
        //Initializing
        hostInput = new InputData();
        clientInput = new InputData();
        coderInput = new InputData();
        artistInput = new InputData();;
        
        //Initializing Unity Input System
        nia = new NetworkInputActions();
        nia.Player.Enable();
    }
    
    private void Update()
    {
        GetInputFromHost();
        GetInputFromClient();
        DecideForInputSource();
        
        //clientInput parameter in this method is the input coming from the client side
        if (!IsHost) GetInputFromClientServerRpc(clientInput);
    }
    
    /// <summary>
    /// <para>Gets input from the host side</para>
    /// <para>Works only in host side</para>
    /// </summary>
    private void GetInputFromHost()
    {
        if (!IsHost) return;
        
        hostInput.moveInput = nia.Player.Move.ReadValue<Vector2>();
        hostInput.isRotatingKey = nia.Player.Rotate.IsPressed();
        hostInput.isJumpKeyDown = nia.Player.Jump.WasPressedThisFrame();
    }
    
    /// <summary>
    /// <para>Gets input from the client side</para>
    /// <para>Doesn't work in host side</para>
    /// </summary>
    private void GetInputFromClient()
    {
        if (IsHost) return;
        
        clientInput.moveInput = nia.Player.Move.ReadValue<Vector2>();
        clientInput.isRotatingKey = nia.Player.Rotate.IsPressed();
        clientInput.isJumpKeyDown = nia.Player.Jump.WasPressedThisFrame();
    }
    
    /// <summary>
    /// <para>Decides coder and artist input in order to which one is host and which one is client</para>
    /// <para>Works and should work both in host side and client side</para>
    /// </summary>
    private void DecideForInputSource()
    {
        if (NetworkData.isHostCoder.Value)
        {
            coderInput = hostInput;
            artistInput = clientInput;
        }
        
        else
        {
            coderInput = clientInput;
            artistInput = hostInput;
        }
    }
    
    /// <summary>
    /// <para>Shouldn't work in host side</para>
    /// <para>Sends input from the client side to host side</para>
    /// <param name="inputFromClient">Input from client side that will be send to server side</param>
    /// </summary>
    [ServerRpc(RequireOwnership = false)]
    private void GetInputFromClientServerRpc(InputData inputFromClient)
    {
        clientInput = inputFromClient;
    }
}