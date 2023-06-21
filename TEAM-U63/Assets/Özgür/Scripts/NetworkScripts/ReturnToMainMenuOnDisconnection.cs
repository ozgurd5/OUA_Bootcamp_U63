using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// <para>Returns player to the main menu after disconnection</para>
/// </summary>
public class ReturnToMainMenuOnDisconnection : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        
        if (IsHost)
            NetworkManager.Singleton.OnServerStopped += obj => ReturnToMainMenu();
        else
            NetworkManager.Singleton.OnClientStopped += obj => ReturnToMainMenu();
    }

    /// <summary>
    /// <para>Returns player to the main menu after disconnection</para>
    /// </summary>
    private void ReturnToMainMenu()
    {
        SceneManager.LoadScene("LobbyScene", LoadSceneMode.Single);
        
        foreach (GameObject item in CustomDontDestroyOnLoad.DontDestroyOnLoadList)
            Destroy(item);
    }
}