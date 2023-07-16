using UnityEngine;

/// <summary>
/// <para>CustomDontDestroyOnLoad.cs script destroys all of its objects when main menu is "active". It should destroy
/// objects when we "return" to the main menu but my code doesn't work like that. NetworkManager is starting the
/// game in the main menu, so DontDestroyOnLoad.cs tries to delete it even when we are just start the game</para>
/// <para>To fix this issue first I make CustomDontDestroyOnLoad doesn't destroy NetworkManager, but that creates
/// another problem: When player "return" to the main menu, NetworkManager won't be destroyed, there are two objects</para>
/// <para>This script destroys the other one and works with CustomNetworkManager too</para>
/// </summary>
public class NetworkManagerDestroyer : MonoBehaviour
{
    private static NetworkManagerDestroyer Singleton;

    private void Awake()
    {
        if (Singleton == null) Singleton = this;
        else Destroy(gameObject);
    }
}
