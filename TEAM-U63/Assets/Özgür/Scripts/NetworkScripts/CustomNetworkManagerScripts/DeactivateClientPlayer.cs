using Unity.Netcode;
using UnityEngine;

/// <summary>
/// <para>Deactivates character controlled by client when client disconnects</para>
/// <para>Works only in host side</para>
/// </summary>
public class DeactivateClientPlayer : NetworkBehaviour
{
    [Header("Assign")]
    [SerializeField] private GameObject coderPlayer;
    [SerializeField] private GameObject artistPlayer;

    private NetworkPlayerData npd;

    public override void OnNetworkSpawn()
    {
        if (!IsHost) return;
        
        base.OnNetworkSpawn();
        
        npd = NetworkPlayerData.Singleton;

        NetworkManager.Singleton.OnClientConnectedCallback += obj =>
        {
            //Since host is also a client, creating of the lobby will trigger this event
            //We can prevent that by not counting host as a client
            if (NetworkManager.Singleton.ConnectedClientsList.Count == 1) return;
            
            coderPlayer.SetActive(true);
            artistPlayer.SetActive(true);
        };

        //Disconnection of the host not important since it will disconnect the client automatically because server is gone
        NetworkManager.Singleton.OnClientDisconnectCallback += obj =>
        {
            //Simply deactivates the player controlled by client
            coderPlayer.SetActive(npd.isHostCoder);
            artistPlayer.SetActive(!npd.isHostCoder);
        };
    }
}