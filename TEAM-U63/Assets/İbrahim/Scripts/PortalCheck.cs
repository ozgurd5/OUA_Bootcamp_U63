using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalCheck : NetworkBehaviour
{
    [Header("Assign")]
    [SerializeField] private int requiredPlayers = 2;
    
    private int numberOfPlayers;

    private void Update()
    {
        if (requiredPlayers != numberOfPlayers) return;

        NetworkManager.Singleton.SceneManager.LoadScene("TEST", LoadSceneMode.Single);
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