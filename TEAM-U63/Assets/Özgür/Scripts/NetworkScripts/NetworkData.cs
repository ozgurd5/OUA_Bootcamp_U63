using Unity.Netcode;

//TODO -: isHostCoder shouldn't be a network variable that consumes bandwidth
//TODO -: Create an action called OnPlayersChanged and subscribe a ServerRPC and a ClientRPC called SyncSelectedPlayers to it.
//TODO -: SyncSelectedPlayers must change isHostCoder variable. If host change player, call the ClientRPC. If not, call the ServerRPC.

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