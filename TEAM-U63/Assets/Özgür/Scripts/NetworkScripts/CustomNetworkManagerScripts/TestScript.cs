using Unity.Netcode;
using UnityEngine;

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
       
        coder = GameObject.Find("CoderPlayer").GetComponent<PlayerData>();
        artist = GameObject.Find("ArtistPlayer").GetComponent<PlayerData>();
        coderPsd = coder.GetComponent<PlayerStateData>();
        artistPsd = artist.GetComponent<PlayerStateData>();
        
        NetworkManager.Singleton.StartHost();
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J)) NetworkManager.Singleton.StartHost();
        else if (Input.GetKeyDown(KeyCode.K)) NetworkManager.Singleton.StartClient();
        
        else if (Input.GetKeyDown(KeyCode.L))
        {
            if (coderPsd.currentMainState == PlayerStateData.PlayerMainState.NormalState &&
                artistPsd.currentMainState == PlayerStateData.PlayerMainState.NormalState
                && !artistPsd.isGrabbing && !coderPsd.isGrabbing)
            {
                npd.UpdateIsHostCoder(!npd.isHostCoder);
                coder.DecideControlSource();
                artist.DecideControlSource();
            }
        }
    }
}
