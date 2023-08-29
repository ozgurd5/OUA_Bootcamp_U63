using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroCutsceneManager : NetworkBehaviour
{
    private void Start()
    {
        Invoke(nameof(LoadMainIsland), 35f);
    }

    private void Update()
    {
        //TODO: BETTER
        if (Input.GetKeyDown(KeyCode.Space)) LoadMainIsland();
    }

    private void LoadMainIsland()
    {
        NetworkManager.SceneManager.LoadScene("Main Island", LoadSceneMode.Single);
    }
}
