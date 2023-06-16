using TMPro;
using UnityEngine;
using UnityEngine.UI;

//TODO: create an UI text box that prints the error message
//TODO: allow player to copy the joinCode in the game

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
        //joinCode comes from TMP Input Field with an empty character " ​ " in the end of the string, idk why
        //To prevent that just took first 6 character
        if (enterLobbyJoinCodeText.text != "​")
            joinCode = enterLobbyJoinCodeText.text.Substring(0, 6);

        createdLobbyJoinCodeText.text = UnityRelayServiceManager.joinCode;
    }
}
