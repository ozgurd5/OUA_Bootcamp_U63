using UnityEngine;

/// <summary>
/// <para>Responsible of map control mechanic in the first island's second level (labyrinth)</para>
/// <para>Works only for local player</para>
/// </summary>
public class PlayerMapController : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private GameObject mapCanvas;

    private PlayerData pd;
    private PlayerInputManager pim;
    
    public bool mapCollected;
    private bool isMapActive;

    private void Awake()
    {
        pd = GetComponent<PlayerData>();
        pim = GetComponent<PlayerInputManager>();
    }

    /// <summary>
    /// <para>Opens and closes the map via input</para>
    /// <para>Must work in Update</para>
    /// </summary>
    private void OpenAndCloseMap()
    {
        if (pim.isMapKeyDown && mapCollected)
        {
            isMapActive = !isMapActive;
            mapCanvas.SetActive(isMapActive);
        }
    }

    /// <summary>
    /// <para>Collects map, must work in OnTriggerEnter</para>
    /// </summary>
    /// <param name="col">Collider coming from OnTriggerEnter</param>
    private void CollectMap(Collider col)
    {
        if (!col.CompareTag("MapCollectible")) return;
        
        Destroy(col.gameObject);
            
        mapCollected = true;
        isMapActive = true;
            
        mapCanvas.SetActive(true);
    }
    
    private void Update()
    {
        if (pd.isLocal) OpenAndCloseMap();
    }
    
    private void OnTriggerEnter(Collider col)
    {
        if (pd.isLocal) CollectMap(col);
        else if (col.CompareTag("MapCollectible")) Destroy(col.gameObject);
    }
}
