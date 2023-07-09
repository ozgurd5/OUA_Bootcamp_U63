using TMPro;
using Unity.Netcode;
using UnityEngine;

public class PlayerNameUI : NetworkBehaviour
{
    [Header("Assign")]
    [SerializeField] private TMP_InputField playerNameInputField;
    [SerializeField] private TextMeshProUGUI coderNameText;
    [SerializeField] private TextMeshProUGUI artistNameText;
    
    private string localPlayerName;

    private string hostName;
    private string clientName;

    private NetworkPlayerData npd;
    
    private void Awake()
    {
        npd = NetworkPlayerData.Singleton;
        
        //Works locally
        playerNameInputField.onValueChanged.AddListener(enteredName => localPlayerName = enteredName);

        //npd.OnIsHostCoderChanged += UpdatePlayerNames;
        NetworkManager.Singleton.OnClientConnectedCallback += obj => SetPlayerNames();
    }
    
    /// <summary>
    /// <para>Sets the player names through the network when a player joins</para>
    /// <para>Works and must work both in host side and client side</para>
    /// </summary>
    private void SetPlayerNames()
    {
        if (IsHost)
        {
            hostName = localPlayerName;
            SendHostNameClientRpc(hostName);
        }

        else
        {
            clientName = localPlayerName;
            SendClientNameServerRpc(clientName);
        }
        
        //This method in this context updates only one player on that player's side. No network sync
        //Because RPC methods are working after SetPlayerNames method is done, they are not multi-threaded
        //So the UpdatePlayerNames methods in RPC methods are responsible of the sync across the network, not this one
        UpdatePlayerNames(npd.isHostCoder);
    }

    /// <summary>
    /// <para>Sends client name to the host side</para>
    /// <para>Must not be called from the host side. Since host is also a client, it can call this method, be careful</para>
    /// </summary>
    /// <param name="clientNameComingFromClient">Client name coming from client side</param>
    [ServerRpc(RequireOwnership = false)]
    private void SendClientNameServerRpc(string clientNameComingFromClient)
    {
        clientName = clientNameComingFromClient;
        UpdatePlayerNames(npd.isHostCoder);
    }
    
    /// <summary>
    /// <para>Sends host name to the client side</para>
    /// <para>Can't work in host side, though it's not important</para>
    /// </summary>
    /// <param name="hostNameComingFromHost">Host name coming from host side</param>
    [ClientRpc]
    private void SendHostNameClientRpc(string hostNameComingFromHost)
    {
        if (IsHost) return;
        hostName = hostNameComingFromHost;
        UpdatePlayerNames(npd.isHostCoder);
    }
    
    /// <summary>
    /// <para>Updates names of the players when host or client change player</para>
    /// <para>Works and must work both in host side and client side</para>
    /// </summary>
    /// <param name="isHostCoder">Current isHostCoder value</param>
    private void UpdatePlayerNames(bool isHostCoder)
    {
        if (isHostCoder)
        {
            coderNameText.text = hostName;
            artistNameText.text = clientName;
        }

        else
        {
            coderNameText.text = clientName;
            artistNameText.text = hostName;
        }
    }
}