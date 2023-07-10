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
    [SerializeField] private float paintRange = 7f;

    private PlayerData pd;
    private PlayerInputManager pim;
    private CrosshairManager cm;
    
    private void Awake()
    {
        pd = GetComponent<PlayerData>();
        pim = GetComponent<PlayerInputManager>();
        cm = GetComponentInChildren<CrosshairManager>();
    }

    private void Update()
    {
        if (!pd.isLocal) return;
        if (!pim.isSecondaryAbilityKeyDown) return;   
        if (!cm.isLookingAtCube) return;
        
        cm.crosshairHit.collider.gameObject.GetComponent<CubeManager>().PaintCube();
    }
}