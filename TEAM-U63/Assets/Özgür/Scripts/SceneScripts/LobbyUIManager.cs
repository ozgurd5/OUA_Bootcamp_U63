using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyUIManager : NetworkBehaviour
{
    [Header("Assign")]
    [SerializeField] private Button startGameButton;
    [SerializeField] private Button selectCoderButton;
    [SerializeField] private Button selectArtistButton;
    
    private void Start()
    {
        startGameButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        });
        
        selectCoderButton.onClick.AddListener(() =>
        {
            if (IsHost)
                NetworkData.isHostCoder.Value = true;
            else
                MakeClientCoderServerRpc(true);
        });
        
        selectArtistButton.onClick.AddListener(() =>
        {
            if (IsHost)
                NetworkData.isHostCoder.Value = false;
            else
                MakeClientCoderServerRpc(false);
        });
    }
    
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (!IsHost) startGameButton.gameObject.SetActive(false);
    }
    
    /// <summary>
    /// <para>Makes client controlled character coder</para>
    /// </summary>
    /// <param name="isClientSelectedCoder">True if client is selected coder</param>
    [ServerRpc(RequireOwnership = false)]
    private void MakeClientCoderServerRpc(bool isClientSelectedCoder)
    {
        if (IsHost) return;
        NetworkData.isHostCoder.Value = !isClientSelectedCoder;
    }
}