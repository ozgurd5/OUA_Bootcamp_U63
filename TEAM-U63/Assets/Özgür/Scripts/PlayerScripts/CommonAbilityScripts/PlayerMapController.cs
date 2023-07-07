using UnityEngine;

/// <summary>
/// <para>Responsible of map control mechanic in the first island's second level (labyrinth)</para>
/// </summary>
public class PlayerMapController : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private GameObject mapCollectible;
    [SerializeField] private GameObject mapCanvas;

    private PlayerController pc;
    
    private bool mapCollected;
    private bool isMapActive;

    private void Awake()
    {
        pc = GetComponent<PlayerController>();
    }

    /// <summary>
    /// <para>Opens and closes the map via input</para>
    /// <para>Must work in Update</para>
    /// </summary>
    private void OpenAndCloseMap()
    {
        if (!pc.input.isMapKeyDown || !mapCollected) return;
        
        isMapActive = !isMapActive;
        mapCanvas.SetActive(isMapActive);
    }

    /// <summary>
    /// <para>Collects map, must work in OnTriggerEnter</para>
    /// </summary>
    /// <param name="col">Collider coming from OnTriggerEnter</param>
    private void CollectMap(Collider col)
    {
        if (col.gameObject != mapCollectible) return;
        
        Destroy(mapCollectible);
            
        mapCollected = true;
        isMapActive = true;
            
        mapCanvas.SetActive(true);
    }
    
    private void Update()
    {
        OpenAndCloseMap();   
    }
    
    private void OnTriggerEnter(Collider col)
    {
        CollectMap(col);
    }
}
