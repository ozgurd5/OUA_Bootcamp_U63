using UnityEngine;

public class PortalCheck : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private int requiredPlayers = 2;
    
    private int numberOfPlayers;

    private void Update()
    {
        if (requiredPlayers != numberOfPlayers) return;
        
        Debug.Log("both players are in");
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