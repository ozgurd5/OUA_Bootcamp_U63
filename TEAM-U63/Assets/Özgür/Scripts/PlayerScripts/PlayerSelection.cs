using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelection : NetworkBehaviour
{
    [Header("Assign - Buttons")]
    [SerializeField] private Button selectCoderButton;
    [SerializeField] private Button selectArtistButton;

    public static NetworkVariable<bool> isHostCoder;

    private void Awake()
    {
        isHostCoder = new NetworkVariable<bool>(true);

        selectCoderButton.onClick.AddListener(() =>
        {
            if (IsHost)
                isHostCoder.Value = IsHost;
            else
                ChangeClientPlayerServerRpc(true);
        });
        
        selectArtistButton.onClick.AddListener(() =>
        {
            if (IsHost)
                isHostCoder.Value = !IsHost;
            else
                ChangeClientPlayerServerRpc(false);
        });
    }
    
    /// <summary>
    /// <para>Changes character that clients control</para>
    /// </summary>
    /// <param name="isClientSelectedCoder">True if client is selected coder</param>
    [ServerRpc(RequireOwnership = false)]
    private void ChangeClientPlayerServerRpc(bool isClientSelectedCoder)
    {
        isHostCoder.Value = !isClientSelectedCoder;
    }
}