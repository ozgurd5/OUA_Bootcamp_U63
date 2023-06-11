using TMPro;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI : NetworkBehaviour
{
    [Header("Assign Manually")]
    [SerializeField] private GameObject host;
    [SerializeField] private GameObject client;
    [SerializeField] private GameObject cube;
    
    [Header("Buttons")]
    [SerializeField] private Button serverButton;
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;
    [SerializeField] private Button toggleClientInterButton;
    
    [Header("Info Text")]
    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private TextMeshProUGUI isOwnerText;
    [SerializeField] private TextMeshProUGUI clientInter;
    [SerializeField] private TextMeshProUGUI hostPosition;
    [SerializeField] private TextMeshProUGUI clientPosition;
    [SerializeField] private TextMeshProUGUI cubePosition;
    
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

        toggleClientInterButton.onClick.AddListener(() =>
        {
            client.GetComponent<NetworkTransform>().Interpolate = !client.GetComponent<NetworkTransform>().Interpolate;
        });
    }
    
    private void Update()
    {
        infoText.text = "IsServer: " + IsServer + "\n" + "IsHost: " + IsHost + "\n" + "IsClient: " + IsClient;
        
        clientInter.text = "Client interpolation: " + client.GetComponent<NetworkTransform>().Interpolate;

        hostPosition.text = "Host position: " + host.transform.position;
        clientPosition.text = "Client position: " + client.transform.position;
        cubePosition.text = "Cube position: " + cube.transform.position;
        
        if (isOwnerCheck != null)
            isOwnerText.text = "IsOwner: " + isOwnerCheck.IsOwner;
        else
            isOwnerText.text = "Assign a NetworkObject script from Unity Inspector";
    }
}
