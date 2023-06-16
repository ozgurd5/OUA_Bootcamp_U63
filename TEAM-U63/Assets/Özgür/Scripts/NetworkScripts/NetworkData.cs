using Unity.Netcode;

/// <summary>
/// <para>Variables and actions necessary for the network</para>
/// </summary>
public class NetworkData : NetworkBehaviour
{
    public static bool isClientInGame;
    public static NetworkVariable<bool> isHostCoder = new NetworkVariable<bool>(true);

    private void Update()
    {
        if (!IsHost) return;
        
        isClientInGame = NetworkManager.Singleton.ConnectedClientsList.Count != 1;
    }
}