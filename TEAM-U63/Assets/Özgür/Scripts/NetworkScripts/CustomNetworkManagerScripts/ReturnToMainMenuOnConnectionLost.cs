using Unity.Netcode;
using UnityEngine.SceneManagement;

/// <summary>
/// <para>Returns player to the main menu after connection lost</para>
/// <para>Works both in host and client side</para>
/// </summary>
public class ReturnToMainMenuOnConnectionLost : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        
        if (IsHost) NetworkManager.Singleton.OnServerStopped += obj => ReturnToMainMenu();
        else NetworkManager.Singleton.OnClientStopped += obj => ReturnToMainMenu();
    }

    /// <summary>
    /// <para>Returns player to the main menu after disconnection</para>
    /// </summary>
    private void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MAIN_MENU", LoadSceneMode.Single);
    }
}