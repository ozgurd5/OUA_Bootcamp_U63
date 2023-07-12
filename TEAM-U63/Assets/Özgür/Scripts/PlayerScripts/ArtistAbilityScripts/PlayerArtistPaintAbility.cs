using Unity.Netcode;

/// <summary>
/// <para>Responsible of painting ability of the artist</para>
/// <para>Works only in local player</para>
/// </summary>
public class PlayerArtistPaintAbility : NetworkBehaviour
{
    private PlayerInputManager pim;
    private CrosshairManager cm;
    
    private void Awake()
    {
        pim = GetComponent<PlayerInputManager>();
        cm = GetComponentInChildren<CrosshairManager>();
    }

    private void Update()
    {
        if (!pim.isSecondaryAbilityKeyDown) return;   
        if (!cm.isLookingAtCube) return;
        
        cm.crosshairHit.collider.gameObject.GetComponent<CubeManager>().PaintCube();
    }
}