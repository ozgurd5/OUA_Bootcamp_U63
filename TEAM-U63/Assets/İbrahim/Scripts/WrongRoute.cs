using UnityEngine;

public class WrongRoute : MonoBehaviour
{
    public GameObject respawnPoint;

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player")) col.transform.position = respawnPoint.transform.position;
    }
}