using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// <para>Responsible of player selection and starting of the game</para>
/// </summary>
public class LobbyUI : NetworkBehaviour
{
    [Header("Assign")]
    [SerializeField] private Button startGameButton;

    [SerializeField] private Button switchPlayerButton;

    private NetworkPlayerData npd;

    private void Awake()
    {
        npd = NetworkPlayerData.Singleton;
        
        startGameButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        });
        
        switchPlayerButton.onClick.AddListener(() => { npd.UpdateIsHostCoder(!npd.isHostCoder); });
    }
    
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        
        //Client shouldn't start the game
        if (IsHost) startGameButton.gameObject.SetActive(true);
    }
}