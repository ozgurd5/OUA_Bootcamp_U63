using Unity.Netcode;
using UnityEngine;

/// <summary>
/// <para>Responsible of painting ability of the artist</para>
/// <para>Works only in local player</para>
/// </summary>
public class PlayerArtistPaintAbility : NetworkBehaviour
{
    [Header("ASSIGN - BROKEN")]
    [SerializeField] private CrosshairManager cm;
    
    [Header("Assign - Coder Vision Audio Source")]
    [SerializeField] private AudioSource aus;

    private PlayerInputManager pim;
    
    private void Awake()
    {
        pim = GetComponent<PlayerInputManager>();
    }

    private void Update()
    {
        if (!pim.isSecondaryAbilityKeyDown) return;

        if (cm.isLookingAtCube)
        {
            cm.crosshairHit.collider.GetComponent<CubeManager>().PaintCube();
            aus.Play();
        }
        
        else if (cm.isLookingAtRobot)
        {
            cm.crosshairHit.collider.GetComponent<RobotManager>().PaintRobot();
            aus.Play();
        }
    }
}