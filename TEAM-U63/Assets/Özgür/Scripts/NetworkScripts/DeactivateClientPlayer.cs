using Unity.Netcode;
using UnityEngine;

public class DeactivateClientPlayer : NetworkBehaviour
{
    [Header("Assign")]
    [SerializeField] private GameObject coderPlayer;
    [SerializeField] private GameObject artistPlayer;

    private void Update()
    {
        if (!IsHost) return;
        
        NetworkData.isClientInGame = NetworkManager.Singleton.ConnectedClientsList.Count != 1;
        
        if (NetworkData.isHostCoder.Value)
        {
            artistPlayer.SetActive(NetworkData.isClientInGame);
        }

        else
        {
            coderPlayer.SetActive(NetworkData.isClientInGame);
        }
    }
}