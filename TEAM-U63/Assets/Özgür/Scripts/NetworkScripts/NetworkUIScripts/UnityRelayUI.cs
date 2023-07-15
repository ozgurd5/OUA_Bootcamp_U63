using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// <para>Handles creating and joining a lobby</para>
/// <para>Transport in network manager must be Relay Unity Transport</para>
/// <para>Works both in host and client side</para>
/// </summary>
public class UnityRelayUI : NetworkBehaviour
{
    [Header("Assign")]
    [SerializeField] private Button createLobbyButton;
    [SerializeField] private Button joinLobbyButton;
    [SerializeField] private TMP_InputField enterLobbyJoinCodeText;
    
    [Header("Assign - Join Code Elements")]
    [SerializeField] private TextMeshProUGUI lobbyJoinCodeText;
    [SerializeField] private GameObject copyLobbyJoinCodeButton;
    [SerializeField] private GameObject lobbyIDText;
    
    private string joinCodeComingFromClient;
    
    private void Awake()
    {
        createLobbyButton.onClick.AddListener(UnityRelayServiceManager.CreateRelay);
        enterLobbyJoinCodeText.onValueChanged.AddListener(clientInput => joinCodeComingFromClient = clientInput);
        joinLobbyButton.onClick.AddListener(() => UnityRelayServiceManager.JoinRelay(joinCodeComingFromClient)); 
        
        //Creation of the lobby join code is async. We have to wait Unity Relay Service to create a code for us
        //That means we can't write lobby join code to UI until it's created
        UnityRelayServiceManager.OnLobbyJoinCodeCreated += WriteLobbyJoinCodeToUI;
    }
    
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsHost)
        {
            lobbyJoinCodeText.gameObject.SetActive(false);
            copyLobbyJoinCodeButton.SetActive(false);
            lobbyIDText.SetActive(false);
        }
    }
    
    /// <summary>
    /// <para>Writes lobby join code to the UI</para>
    /// </summary>
    private void WriteLobbyJoinCodeToUI()
    {
        lobbyJoinCodeText.text = UnityRelayServiceManager.lobbyJoinCode;
    }
}