using Unity.Netcode;
using UnityEngine;

/// <summary>
/// <para>Deactivates character controlled by client when client disconnects</para>
/// </summary>
public class DeactivateClientPlayer : NetworkBehaviour
{
    [Header("Assign")]
    [SerializeField] private GameObject coderPlayer;
    [SerializeField] private GameObject artistPlayer;
    
    private void Update()
    {
        if (!IsHost) return;
        
        if (NetworkData.isClientInGame)
        {
            coderPlayer.SetActive(true);
            artistPlayer.SetActive(true);
        }
        
        else
        {
            coderPlayer.SetActive(NetworkData.isHostCoder.Value);
            artistPlayer.SetActive(!NetworkData.isHostCoder.Value);
        }
    }
}