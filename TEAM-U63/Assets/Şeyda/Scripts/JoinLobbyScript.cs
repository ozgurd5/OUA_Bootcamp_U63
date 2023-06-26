using Unity.Netcode;
using UnityEngine;

public class JoinLobbyScript : NetworkBehaviour
{
    [SerializeField] private GameObject wrongCodeFeedback;
    [SerializeField] private GameObject joinMenu;
    [SerializeField] private GameObject lobby;
    
    private void Awake()
    {
        UnityRelayServiceManager.OnLobbyJoinCodeCorrect += GoToLobbyScreen;
        UnityRelayServiceManager.OnLobbyJoinCodeWrong += EnableWrongCodeFeedback;
    }

    private void GoToLobbyScreen()
    {
        joinMenu.SetActive(false);
        lobby.SetActive(true);
    }

    private void EnableWrongCodeFeedback()
    {
        wrongCodeFeedback.SetActive(true);
        Invoke(nameof(DisableWrongCodeFeedback), 1.5f);
    }

    private void DisableWrongCodeFeedback()
    {
        wrongCodeFeedback.SetActive(false);
    }
}