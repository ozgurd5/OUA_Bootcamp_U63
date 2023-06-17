using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyUIManager : NetworkBehaviour
{
    [Header("Assign")]
    [SerializeField] private Button startGameButton;
    
    private void Awake()
    {
        if (!IsHost || SceneManager.GetActiveScene().name != "LobbyScene")
            startGameButton.enabled = false;
        
        startGameButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("GameScene");
        });
    }
}