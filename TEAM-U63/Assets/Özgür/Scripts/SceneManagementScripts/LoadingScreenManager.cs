using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreenManager : NetworkBehaviour
{
    private Canvas loadingCanvas;

    private void Awake()
    {
        loadingCanvas = GetComponentInChildren<Canvas>();

        SceneManager.activeSceneChanged += (a, b) => loadingCanvas.enabled = false;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        
        NetworkManager.Singleton.SceneManager.OnLoad += (id, sceneName, mode, operation) => 
        {
            loadingCanvas.enabled = true;
        };
    }
}