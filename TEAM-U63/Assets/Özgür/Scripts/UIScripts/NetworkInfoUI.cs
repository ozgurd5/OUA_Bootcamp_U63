using TMPro;
using Unity.Netcode;
using UnityEngine;

public class NetworkInfoUI : NetworkBehaviour
{
    [Header("Assign")]
    [SerializeField] private GameObject host;
    [SerializeField] private GameObject client;
    [SerializeField] private GameObject cube;
    
    [Header("Assign - Ownership")]
    [SerializeField] private NetworkObject isOwnerCheck;
    
    [Header("Assign - Texts")]
    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private TextMeshProUGUI hostPosition;
    [SerializeField] private TextMeshProUGUI clientPosition;
    [SerializeField] private TextMeshProUGUI cubePosition;
    [SerializeField] private TextMeshProUGUI tickRateText;
    [SerializeField] private TextMeshProUGUI isOwnerText;

    private void Update()
    {
        infoText.text = "IsServer: " + IsServer + "\n" + "IsHost: " + IsHost + "\n" + "IsClient: " + IsClient;

        hostPosition.text = "Host position: " + host.transform.position;
        clientPosition.text = "Client position: " + client.transform.position;
        cubePosition.text = "Cube position: " + cube.transform.position;

        tickRateText.text = "Tick rate: " + NetworkManager.Singleton.NetworkConfig.TickRate;
        
        if (isOwnerCheck != null)
            isOwnerText.text = "IsOwner: " + isOwnerCheck.IsOwner;
        else
            isOwnerText.text = "Assign a NetworkObject script from Unity Inspector";
    }
}
