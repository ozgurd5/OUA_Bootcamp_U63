using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// <para>Handles creating and joining a lobby</para>
/// </summary>
public class UnityRelayUI : MonoBehaviour
{
    [SerializeField] private Button createLobbyButton;
    [SerializeField] private Button joinLobbyButton;
    [SerializeField] private TextMeshProUGUI createdLobbyJoinCodeText;
    [SerializeField] private TextMeshProUGUI enterLobbyJoinCodeText;
    
    private string joinCode;
    
    private void Awake()
    {
        createLobbyButton.onClick.AddListener(UnityRelayServiceManager.CreateRelay);
        joinLobbyButton.onClick.AddListener(() => UnityRelayServiceManager.JoinRelay(joinCode));
    }
    
    private void Update()
    {
        //joinCode comes from TMPro Input Field with an empty character " ​ " in the end of the string, idk why
        //To prevent that just we just took first 6 character. Very stupid way but this is the only solution
        if (enterLobbyJoinCodeText.text != "​" || enterLobbyJoinCodeText.text.Length !< 6)
            joinCode = enterLobbyJoinCodeText.text.Substring(0, 6);
        
        createdLobbyJoinCodeText.text = UnityRelayServiceManager.joinCode;
    }
}
