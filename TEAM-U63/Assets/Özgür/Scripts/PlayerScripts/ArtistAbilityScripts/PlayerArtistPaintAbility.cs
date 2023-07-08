using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// <para>Responsible of painting ability of the artist</para>
/// </summary>
public class PlayerArtistPaintAbility : NetworkBehaviour
{
    //
    private NetworkPlayerData npd;
    
    [Header("Assign")]
    [SerializeField] private Image crosshairImage;
    [SerializeField] private float paintRange = 5f;
    
    private PlayerController pc;
    private Camera cam;
    
    private Ray crosshairRay;
    private RaycastHit crosshairHit;
    
    private void Awake()
    {
        pc = GetComponent<PlayerController>();
        cam = Camera.main;
        
        //
        npd = NetworkPlayerData.Singleton;
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
        //
        if ((npd.isHostCoder && !IsHost) || (!npd.isHostCoder && IsHost))
        {
            if (!pc.input.isSecondaryAbilityKeyDown) return;   
            if (!CastRayForCubes()) return;
        
            crosshairHit.collider.gameObject.GetComponent<CubeManager>().PaintCube();
        }
    }
}