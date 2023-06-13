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
            isHostCoder.Value = IsHost;
        });
        
        selectArtistButton.onClick.AddListener(() =>
        {
            isHostCoder.Value = !IsHost;
        });
    }
}
