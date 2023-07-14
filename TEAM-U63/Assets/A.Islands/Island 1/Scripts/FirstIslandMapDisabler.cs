using UnityEngine;

/// <summary>
/// <para>Disables map mechanic in the end of the island 1</para>
/// </summary>
public class FirstIslandMapDisabler : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        if (!col.CompareTag("Player")) return;

        Destroy(col.GetComponent<PlayerMapController>());
    }
}
