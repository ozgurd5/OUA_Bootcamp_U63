using Unity.Netcode;

public class AutoKickThirdPlayer : NetworkBehaviour
{
    private void Update()
    {
        if (!IsHost) return;
        
        if (NetworkManager.Singleton.ConnectedClientsList.Count > 2)
        {
            NetworkManager.Singleton.DisconnectClient(NetworkManager.Singleton.ConnectedClientsIds[2]);
        }
    }
}