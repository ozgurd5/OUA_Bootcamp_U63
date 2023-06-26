using Unity.Netcode;
using UnityEngine;

/// <summary>
/// <para>Checks for is join code correct or wrong. If correct, enables lobby screen, if not, throws an error</para>
/// </summary>
public class JoinLobbyScript : NetworkBehaviour
{
    [SerializeField] private GameObject wrongCodeFeedback;
    [SerializeField] private GameObject joinMenu;
    [SerializeField] private GameObject lobby;
    
    private void Awake()
    {
        UnityRelayServiceManager.OnLobbyJoinCodeCorrect += EnableLobbyScreen;
        UnityRelayServiceManager.OnLobbyJoinCodeWrong += EnableWrongCodeFeedback;
    }

    /// <summary>
    /// <para>Enables lobby screen, disables joinMenu screen</para>
    /// </summary>
    private void EnableLobbyScreen()
    {
        joinMenu.SetActive(false);
        lobby.SetActive(true);
    }

    /// <summary>
    /// <para>Throws an error message</para>
    /// </summary>
    private void EnableWrongCodeFeedback()
    {
        wrongCodeFeedback.SetActive(true);
        Invoke(nameof(DisableWrongCodeFeedback), 1.5f);
    }
    
    /// <summary>
    /// <para>Disables the error message</para>
    /// </summary>
    private void DisableWrongCodeFeedback()
    {
        wrongCodeFeedback.SetActive(false);
    }
}