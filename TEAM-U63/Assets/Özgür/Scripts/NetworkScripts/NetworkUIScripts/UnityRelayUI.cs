using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// <para>Handles creating and joining a lobby</para>
/// </summary>
public class UnityRelayUI : NetworkBehaviour
{
    [Header("Assign")]
    [SerializeField] private Button createLobbyButton;
    [SerializeField] private Button joinLobbyButton;
    [SerializeField] private TextMeshProUGUI lobbyJoinCodeText;
    [SerializeField] private TextMeshProUGUI enterLobbyJoinCodeText;
    
    private string joinCodeComingFromClient;
    
    private void Awake()
    {
        createLobbyButton.onClick.AddListener(UnityRelayServiceManager.CreateRelay);
        joinLobbyButton.onClick.AddListener(() => UnityRelayServiceManager.JoinRelay(joinCodeComingFromClient));
        
        //Creation of the lobby join code is async. We have to wait Unity Relay Service to create a code for us
        //That means we can't write lobby join code to UI until it's created
        UnityRelayServiceManager.OnLobbyJoinCodeCreated += WriteLobbyJoinCodeToUI;
    }
    
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        
        createLobbyButton.gameObject.SetActive(false);
        joinLobbyButton.gameObject.SetActive(false);
        enterLobbyJoinCodeText.transform.parent.parent.gameObject.SetActive(false);
        if (!IsHost) lobbyJoinCodeText.gameObject.SetActive(false);
    }
    
    private void Update()
    {
        if (IsHost) return;
        
        //joinCode comes from TMPro Input Field with an empty character " ​ " in the end of the string, idk why
        //To prevent that we have to take first 6 character.
        if (enterLobbyJoinCodeText.text.Length > 6)
            joinCodeComingFromClient = enterLobbyJoinCodeText.text.Substring(0, 6);
    }
    
    /// <summary>
    /// <para>Writes lobby join code to the UI</para>
    /// </summary>
    private void WriteLobbyJoinCodeToUI()
    {
        lobbyJoinCodeText.text = $"Lobby Join Code\n{UnityRelayServiceManager.lobbyJoinCode}";
    }
}