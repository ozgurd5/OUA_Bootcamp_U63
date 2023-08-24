using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestScript : MonoBehaviour
{
    private NetworkPlayerData npd;
    private PlayerData coder;
    private PlayerData artist;
    private PlayerStateData coderPsd;
    private PlayerStateData artistPsd;

    private void Start()
    {
        npd = NetworkPlayerData.Singleton;
        SceneManager.activeSceneChanged += (a, currentScene) =>
        {
            if (currentScene.name == "MAIN_MENU") return;
            coder = GameObject.Find("CoderPlayer").GetComponent<PlayerData>();
            artist = GameObject.Find("ArtistPlayer").GetComponent<PlayerData>();
            coderPsd = coder.GetComponent<PlayerStateData>();
            artistPsd = artist.GetComponent<PlayerStateData>();
        };

        NetworkManager.Singleton.StartHost();
    }
    
    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.L)) return;
        
        if (coderPsd.currentMainState == PlayerStateData.PlayerMainState.NormalState &&
            artistPsd.currentMainState == PlayerStateData.PlayerMainState.NormalState)
        {
            npd.UpdateIsHostCoder(!npd.isHostCoder);
            coder.DecideControlSource();
            artist.DecideControlSource();
        }
    }
}
