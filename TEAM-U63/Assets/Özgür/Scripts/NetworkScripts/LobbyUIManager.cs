using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// <para>Responsible of player selection and starting of the game</para>
/// </summary>
public class LobbyUIManager : NetworkBehaviour
{
    [Header("Assign")]
    [SerializeField] private Button startGameButton;
    [SerializeField] private Button selectCoderButton;
    [SerializeField] private Button selectArtistButton;

    private NetworkPlayerData npd;

    private void Start()
    {
        npd = NetworkPlayerData.Singleton;
        
        startGameButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        });
        
        selectCoderButton.onClick.AddListener(() => { npd.UpdateIsHostCoder(IsHost); });
        
        selectArtistButton.onClick.AddListener(() => { npd.UpdateIsHostCoder(!IsHost); });
    }
    
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        
        //Client shouldn't start the game
        if (!IsHost) startGameButton.gameObject.SetActive(false);
    }
}