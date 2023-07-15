using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// <para>Creates and connects local host. Changes controlled players</para>
/// </summary>
public class TestLocalServerAndPlayerManager : MonoBehaviour
{
    private NetworkPlayerData npd;

    private void Awake()
    {
        npd = NetworkPlayerData.Singleton;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
            NetworkManager.Singleton.StartHost();

        if (Input.GetKeyDown(KeyCode.K))
            NetworkManager.Singleton.StartClient();
        
        if (Input.GetKeyDown(KeyCode.L))
            npd.UpdateIsHostCoder(!npd.isHostCoder);

        if (Input.GetKeyDown(KeyCode.N))
            NetworkManager.Singleton.SceneManager.LoadScene("Island 2", LoadSceneMode.Single);
        
        if (Input.GetKeyDown(KeyCode.P))
            NetworkManager.Singleton.SceneManager.LoadScene("MAIN_MENU", LoadSceneMode.Single);
    }
}