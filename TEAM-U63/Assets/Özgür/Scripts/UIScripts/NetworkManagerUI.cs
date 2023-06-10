using TMPro;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI : NetworkBehaviour
{
    [Header("Assign Manually")]
    [SerializeField] private GameObject client;
    
    [Header("Buttons")]
    [SerializeField] private Button serverButton;
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;
    [SerializeField] private Button increaseTickButton;
    [SerializeField] private Button decreaseTickButton;
    [SerializeField] private Button toggleClientInterButton;
    
    [Header("Info Text")]
    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private TextMeshProUGUI isOwnerText;
    [SerializeField] private TextMeshProUGUI tickRate;
    [SerializeField] private TextMeshProUGUI clientInter;
    
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
        
        increaseTickButton.onClick.AddListener(() =>
        {
            NetworkManager.NetworkConfig.TickRate += 10;
        });
        
        decreaseTickButton.onClick.AddListener(() =>
        {
            NetworkManager.NetworkConfig.TickRate -= 10;
        });
        
        toggleClientInterButton.onClick.AddListener(() =>
        {
            client.GetComponent<NetworkTransform>().Interpolate = !client.GetComponent<NetworkTransform>().Interpolate;
        });
    }
    
    private void Update()
    {
        infoText.text = "IsServer: " + IsServer + "\n" + "IsHost: " + IsHost + "\n" + "IsClient: " + IsClient;
        
        tickRate.text = "Tick rate: " + NetworkManager.NetworkConfig.TickRate;
        
        clientInter.text = "Client interpolation: " + client.GetComponent<NetworkTransform>().Interpolate;
        
        if (isOwnerCheck != null)
            isOwnerText.text = "IsOwner: " + isOwnerCheck.IsOwner;
        else
            isOwnerText.text = "Assign a NetworkObject script from Unity Inspector";
    }
}
