using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalSceneManager : NetworkBehaviour
{
    [Header("CHECK PORTAL DESTINATION")]
    [SerializeField] private bool toMainIsland;
    [SerializeField] private bool toFirstIsland;
    [SerializeField] private bool toSecondIsland;
    
    private int requiredPlayers = 2;
    private int numberOfPlayers;

    private void Update()
    {
        if (requiredPlayers != numberOfPlayers) return;

        if (toMainIsland)
            NetworkManager.Singleton.SceneManager.LoadScene("Main Island", LoadSceneMode.Single);
        else if (toFirstIsland)
            NetworkManager.Singleton.SceneManager.LoadScene("Island 1", LoadSceneMode.Single);
        else if (toSecondIsland)
            NetworkManager.Singleton.SceneManager.LoadScene("Island 2", LoadSceneMode.Single);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (!col.CompareTag("Player")) return;
        
        numberOfPlayers++;
    }

    private void OnTriggerExit(Collider col)
    {
        if (!col.CompareTag("Player")) return;
        
        numberOfPlayers--;
    }
}