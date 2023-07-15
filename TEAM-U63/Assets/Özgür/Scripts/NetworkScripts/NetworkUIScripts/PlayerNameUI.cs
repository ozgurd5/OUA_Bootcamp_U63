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
        
        npd.OnIsHostCoderChanged += UpdatePlayerNames;
        npd.OnIsHostCoderChanged += SyncPlayerNames;
    }
    
    //We need a spawned network to subscribe OnClient.. actions
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        
        //Works when the lobby created and client joined the lobby, in both sides
        NetworkManager.Singleton.OnClientConnectedCallback += obj =>
        {;
            //Set the names for the first time, locally
            UpdatePlayerNames();
            
            //Sync the names across the network (and update it in the other side)
            SyncPlayerNames();
        };
    }

    /// <summary>
    /// <para>This method only sends data to the players when they join the lobby</para>
    /// </summary>
    private void SyncPlayerNames()
    {
        if (!IsHost) SendClientNameServerRpc(clientName);
        SendHostNameClientRpc(hostName);
    }

    [ClientRpc]
    private void SendHostNameClientRpc(string hostNameInHostSide)
    {
        if (IsHost) return; //Since host is also a client, this method will also run in host side. No need for that.
        hostName = hostNameInHostSide;
        UpdatePlayerNames();
    }

    /// <summary>
    /// <para>HOST MUST NOT CALL THIS METHOD</para>
    /// </summary>
    [ServerRpc(RequireOwnership = false)]
    private void SendClientNameServerRpc(string clientNameInClientSide)
    {
        clientName = clientNameInClientSide;
        UpdatePlayerNames();
    }

    private void UpdatePlayerNames()
    {
        if (IsHost) hostName = localPlayerName;
        else clientName = localPlayerName;

        if (IsHost)
        {
            if (npd.isHostCoder)
            {
                coderNameText.text = localPlayerName;
                artistNameText.text = clientName;   //get from other side
            }
            
            else
            {
                coderNameText.text = clientName;    //get from other side
                artistNameText.text = localPlayerName;
            }
        }

        else
        {
            if (npd.isHostCoder)
            {
                coderNameText.text = hostName;  //get from other side
                artistNameText.text = localPlayerName;
            }
            
            else
            {
                coderNameText.text = localPlayerName;
                artistNameText.text = hostName; //get from other side
            }
        }
    }
}