using Unity.Netcode;
using UnityEngine.SceneManagement;

public class OutroCutsceneManager : NetworkBehaviour
{
    private void Start()
    {
        Invoke(nameof(LoadMainIsland), 5f);
    }

    private void LoadMainIsland()
    {
        NetworkManager.SceneManager.LoadScene("MAIN_MENU", LoadSceneMode.Single);
    }
}
