using UnityEngine;
using UnityEngine.SceneManagement;

public class TestScript : MonoBehaviour
{
    private NetworkPlayerData npd;
    private PlayerData coder;
    private PlayerData artist;

    private void Awake()
    {
        npd = NetworkPlayerData.Singleton;
        
        SceneManager.activeSceneChanged += (a, currentScene) =>
        {
            if (currentScene.name == "MAIN_MENU") return;
            coder = GameObject.Find("CoderPlayer").GetComponent<PlayerData>();
            artist = GameObject.Find("ArtistPlayer").GetComponent<PlayerData>();
        };
    }

    private void Update()
    {
        //TODO: CBB
        if (Input.GetKeyDown(KeyCode.B))
        {
            npd.UpdateIsHostCoder(!npd.isHostCoder);
            coder.DecideControlSource();
            artist.DecideControlSource();
        }
    }
}
