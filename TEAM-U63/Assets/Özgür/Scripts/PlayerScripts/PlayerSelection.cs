using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelection : NetworkBehaviour
{
    [Header("Assign - Buttons")]
    [SerializeField] private Button selectCoderButton;
    [SerializeField] private Button selectArtistButton;

    private void Awake()
    {
        selectCoderButton.onClick.AddListener(() =>
        {
            if (!IsHost)
                MakeClientCoderServerRpc(true);
            else
                NetworkData.isHostCoder.Value = true;
        });
        
        selectArtistButton.onClick.AddListener(() =>
        {
            if (!IsHost)
                MakeClientCoderServerRpc(false);
            else
                NetworkData.isHostCoder.Value = false;
        });
    }
    
    /// <summary>
    /// <para>Makes client controlled character coder</para>
    /// </summary>
    /// <param name="isClientSelectedCoder">True if client is selected coder</param>
    [ServerRpc(RequireOwnership = false)]
    private void MakeClientCoderServerRpc(bool isClientSelectedCoder)
    {
        NetworkData.isHostCoder.Value = !isClientSelectedCoder;
    }
}