using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// <para>Responsible of painting ability of the artist</para>
/// <para>Works only in local player</para>
/// </summary>
public class PlayerArtistPaintAbility : NetworkBehaviour
{
    [Header("Assign")]
    [SerializeField] private Image crosshairImage;
    [SerializeField] private float paintRange = 5f;

    private PlayerData pd;
    private PlayerInputManager pim;
    private Camera cam;
    
    private Ray crosshairRay;
    private RaycastHit crosshairHit;
    
    private void Awake()
    {
        pd = GetComponent<PlayerData>();
        pim = GetComponent<PlayerInputManager>();
        cam = Camera.main;
    }

    /// <summary>
    /// <para>Casts ray for cubes</para>
    /// </summary>
    /// <returns>True if a ray hits a cube</returns>
    private bool CastRayForCubes()
    {
        crosshairRay = cam.ScreenPointToRay(crosshairImage.rectTransform.position);
        bool wasRayHit = Physics.Raycast(crosshairRay, out crosshairHit, paintRange);

        if (!wasRayHit) return false;
        
        Collider col = crosshairHit.collider; //Shorter return statement :p
        return col.CompareTag("RedPuzzle") || col.CompareTag("GreenPuzzle") || col.CompareTag("BluePuzzle");
    }

    private void Update()
    {
        if (!pd.isLocal) return;
        if (!pim.isSecondaryAbilityKeyDown) return;   
        if (!CastRayForCubes()) return;
        
        crosshairHit.collider.gameObject.GetComponent<CubeManager>().PaintCube();
    }
}