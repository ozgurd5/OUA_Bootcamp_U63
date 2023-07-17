using Unity.Netcode;
using UnityEngine.SceneManagement;

public class IntroCutsceneManager : NetworkBehaviour
{
    private void Start()
    {
        Invoke(nameof(LoadMainIsland), 35f);
    }

    private void LoadMainIsland()
    {
        NetworkManager.SceneManager.LoadScene("Main Island", LoadSceneMode.Single);
    }
}
