using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI : NetworkBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button serverButton;
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;

    [Header("Info Text")]
    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private TextMeshProUGUI isOwnerText;

    [Header("IsOwnerCheck")]
    [SerializeField] private NetworkObject isOwnerCheck;

    private void Awake()
    {
        serverButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartServer();
        });
        
        hostButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
        });
        
        clientButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
        });
    }

    private void Update()
    {
        infoText.text = "IsServer: " + IsServer + "\n" + "IsHost: " + IsHost + "\n" + "IsClient: " + IsClient;
        
        if (isOwnerCheck != null)
            isOwnerText.text = "IsOwner: " + isOwnerCheck.IsOwner;
        else
            isOwnerText.text = "Assign a NetworkObject script from Unity Inspector";
    }
}
