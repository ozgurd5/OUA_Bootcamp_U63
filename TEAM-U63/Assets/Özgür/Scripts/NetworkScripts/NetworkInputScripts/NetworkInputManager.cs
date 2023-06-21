using Unity.Netcode;
using UnityEngine;

/// <summary>
/// <para>Gets host and client input. Decides for coder and artist input</para>
/// </summary>
public class NetworkInputManager : NetworkBehaviour
{
    public static NetworkInputManager Singleton;
    
    private NetworkInputActions nia;
    private NetworkPlayerData npd;

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
    
    private InputData hostInput;
    private InputData clientInput;
    
    public InputData coderInput;
    public InputData artistInput;

    private void Start()
    {
        Singleton = GetComponent<NetworkInputManager>();
        
        hostInput = new InputData();
        clientInput = new InputData();
        coderInput = new InputData();
        artistInput = new InputData();;
        
        nia = new NetworkInputActions();
        nia.Player.Enable();

        npd = NetworkPlayerData.Singleton;
    }
    
    private void Update()
    {
        GetInputFromHost();
        GetInputFromClient();
        DecideForInputSource();
        
        //clientInput parameter in this method is the input coming from the client side
        if (!IsHost) SendInputFromClientServerRpc(clientInput);
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
    /// <para>Works only in client side</para>
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
    /// <para>Works and must work both in host side and client side</para>
    /// </summary>
    private void DecideForInputSource()
    {
        if (npd.isHostCoder)
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
    /// <para>Sends input from the client side to host side</para>
    /// <para>Must not be called from the host side. Since host is also a client, it can call this method, be careful</para>
    /// <param name="inputFromClient">Input from client side that will be send to server side</param>
    /// </summary>
    [ServerRpc(RequireOwnership = false)]
    private void SendInputFromClientServerRpc(InputData inputFromClient)
    {
        clientInput = inputFromClient;
    }
}