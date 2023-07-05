using UnityEngine;

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

    private void Update()
    {
        if (!pc.input.isMapKeyDown || !mapCollected) return;
        
        isMapActive = !isMapActive;
            
        mapCanvas.SetActive(isMapActive);
    }
    
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject == mapCollectible)
        {
            Destroy(mapCollectible);
            
            mapCollected = true;
            isMapActive = true;
            
            mapCanvas.SetActive(true);
        }
    }
}
