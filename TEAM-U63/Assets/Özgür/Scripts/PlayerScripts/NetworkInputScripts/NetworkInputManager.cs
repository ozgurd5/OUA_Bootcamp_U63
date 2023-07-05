using Unity.Netcode;
using UnityEngine;

//TODO: fix OnNetworkSpawn before build

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
        public bool isRunKey;
        public bool isGrabKeyDown;
        public bool isPrimaryAbilityKeyDown;
        public bool isSecondaryAbilityKeyDown;
        public bool isEasterEggKeyDown;
        public bool isEasterEggKeyUp;
        public bool isMapKeyDown;

        public bool robotIsAscendKeyDown;
        public bool robotIsDescendKeyDown;

        //We must teach Unity how to serialize InputData objects in order to transfer data in the network
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref moveInput);
            serializer.SerializeValue(ref isRunKey);
            serializer.SerializeValue(ref isGrabKeyDown);
            serializer.SerializeValue(ref isPrimaryAbilityKeyDown);
            serializer.SerializeValue(ref isSecondaryAbilityKeyDown);
            serializer.SerializeValue(ref isEasterEggKeyDown);
            serializer.SerializeValue(ref isEasterEggKeyUp);
            serializer.SerializeValue(ref isMapKeyDown);
            
            serializer.SerializeValue(ref robotIsAscendKeyDown);
            serializer.SerializeValue(ref robotIsDescendKeyDown);
        }
    }
    
    private InputData hostInput;
    private InputData clientInput;
    
    public InputData coderInput;
    public InputData artistInput;
    public InputData robotInput;

    private void Awake()
    {
        Singleton = GetComponent<NetworkInputManager>();
        
        hostInput = new InputData();
        clientInput = new InputData();
        coderInput = new InputData();
        artistInput = new InputData();
        robotInput = new InputData();
        
        nia = new NetworkInputActions();
        nia.Player.Enable();

        npd = NetworkPlayerData.Singleton;
        
        DecideForInputSource();
        npd.OnIsHostCoderChanged += obj => DecideForInputSource();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        
        //DecideForInputSource();
    }

    private void Update()
    {
        GetInputFromHost();
        GetInputFromClient();

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
        
        hostInput.moveInput = nia.Player.Movement.ReadValue<Vector2>();
        hostInput.isRunKey = nia.Player.Run.IsPressed();
        hostInput.isGrabKeyDown = nia.Player.Grab.WasPressedThisFrame();
        hostInput.isPrimaryAbilityKeyDown = nia.Player.PrimaryAbility.WasPressedThisFrame();
        hostInput.isSecondaryAbilityKeyDown = nia.Player.SecondaryAbility.WasPressedThisFrame();
        hostInput.isEasterEggKeyDown = nia.Player.EasterEgg.WasPressedThisFrame();
        hostInput.isEasterEggKeyUp = nia.Player.EasterEgg.WasReleasedThisFrame();
        hostInput.isMapKeyDown = nia.Player.MapKey.WasPressedThisFrame();

        hostInput.robotIsAscendKeyDown = nia.Robot.Ascend.WasPressedThisFrame();
        hostInput.robotIsDescendKeyDown = nia.Robot.Descend.WasPressedThisFrame();
    }
    
    /// <summary>
    /// <para>Gets input from the client side</para>
    /// <para>Works only in client side</para>
    /// </summary>
    private void GetInputFromClient()
    {
        if (IsHost) return;
        
        clientInput.moveInput = nia.Player.Movement.ReadValue<Vector2>();
        clientInput.isRunKey = nia.Player.Run.IsPressed();
        clientInput.isGrabKeyDown = nia.Player.Grab.WasPressedThisFrame();  
        clientInput.isPrimaryAbilityKeyDown = nia.Player.PrimaryAbility.WasPressedThisFrame();
        clientInput.isSecondaryAbilityKeyDown = nia.Player.SecondaryAbility.WasPressedThisFrame();
        clientInput.isEasterEggKeyDown = nia.Player.EasterEgg.WasPressedThisFrame();
        clientInput.isEasterEggKeyUp = nia.Player.EasterEgg.WasReleasedThisFrame();
        clientInput.isMapKeyDown = nia.Player.MapKey.WasPressedThisFrame();
        
        clientInput.robotIsAscendKeyDown = nia.Robot.Ascend.WasPressedThisFrame();
        clientInput.robotIsDescendKeyDown = nia.Robot.Descend.WasPressedThisFrame();
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
    /// <param name="inputFromClient">Input from client side that will be send to host side</param>
    /// </summary>
    [ServerRpc(RequireOwnership = false)]
    private void SendInputFromClientServerRpc(InputData inputFromClient)
    {
        clientInput = inputFromClient;
    }
}