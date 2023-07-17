using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalSceneManager : NetworkBehaviour
{
    [Header("Assign Vortex")]
    [SerializeField] private GameObject vortex;
    
    [Header("CHECK PORTAL DESTINATION")]
    [SerializeField] private bool toMainIsland;
    [SerializeField] private bool toFirstIsland;
    [SerializeField] private bool toSecondIsland;
    [SerializeField] private bool toMainMenu;
    [SerializeField] private bool toTEST;

    [Header("Info - No touch")]
    [SerializeField] private string previousSceneName;
    [SerializeField] private string currentSceneName;
    
    private int requiredPlayers = 2;
    private int numberOfPlayers;

    private GameAnalyticsManager gam;

    private void Awake()
    {
        gam = GameObject.Find("CustomNetworkManager").GetComponent<GameAnalyticsManager>();
        
        if (toSecondIsland)
        {
            vortex.SetActive(gam.isFirstIslandCompleted);
            toSecondIsland = gam.isFirstIslandCompleted;
        }
        
        else if (toMainMenu)
        {
            vortex.SetActive(gam.isSecondIslandCompleted);
            toMainMenu = gam.isSecondIslandCompleted;
        }
    }

    //TODO make it better using events
    private void Update()
    {
        if (requiredPlayers != numberOfPlayers) return;

        if (toMainIsland) NetworkManager.Singleton.SceneManager.LoadScene("Main Island", LoadSceneMode.Single);
        else if (toFirstIsland)
            NetworkManager.Singleton.SceneManager.LoadScene("Island 1", LoadSceneMode.Single);
        else if (toSecondIsland)
            NetworkManager.Singleton.SceneManager.LoadScene("Island 2", LoadSceneMode.Single);
        else if (toMainMenu)
            NetworkManager.Singleton.SceneManager.LoadScene("OutroCutscene", LoadSceneMode.Single);
        else if (toTEST)
            NetworkManager.Singleton.SceneManager.LoadScene("TEST", LoadSceneMode.Single);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (!col.CompareTag("Player")) return;
        
        numberOfPlayers++;
    }

    private void OnTriggerExit(Collider col)
    {
        if (!col.CompareTag("Player")) return;
        
        numberOfPlayers--;
    }
}