using UnityEngine;

/// <summary>
/// <para>CustomDontDestroyOnLoad.cs script destroys all of its objects when main menu is "active". It should destroy
/// objects when we "return" to the main menu but my code doesn't work like that. CustomNetworkManager is starting the
/// game in the main menu, so DontDestroyOnLoad.cs tries to delete it even when we are just start the game</para>
/// <para>To fix this issue first I make CustomDontDestroyOnLoad doesn't destroy CustomNetworkManager, but that creates
/// another problem: When player "return" to the main menu, CustomNetworkManager won't be destroyed, there are two objects</para>
/// <para>This script destroys the other one and works with NetworkManager too</para>
/// </summary>
public class CustomNetworkManagerDestroyer : MonoBehaviour
{
    private static CustomNetworkManagerDestroyer Singleton;

    private void Awake()
    {
        if (Singleton == null) Singleton = this;
        else Destroy(gameObject);
    }
}
