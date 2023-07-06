using Cinemachine;
using DG.Tweening;
using UnityEngine;

/// <summary>
/// <para>Controls easter egg state</para>
/// </summary>
public class EasterEggController : MonoBehaviour
{
    private PlayerStateData psd;
    private PlayerController pc;
    
    private Rigidbody rb;
    private Transform cameraPivot;
    private float defaultCameraPivotLocalYPosition;

    private void Awake()
    {
        psd = GetComponent<PlayerStateData>();
        pc = GetComponent<PlayerController>();

        rb = GetComponent<Rigidbody>();
        cameraPivot = GetCameraPivotTransform();
        defaultCameraPivotLocalYPosition = cameraPivot.localPosition.y;

        pc.OnEasterEggEnter += EnterEasterEggState;
        pc.OnEasterEggExit += ExitEasterEggState;
    }

    /// <summary>
    /// <para>Gets camera pivot's transform in order to assigned player</para>
    /// </summary>
    /// <returns>Coder's camera pivot if the player is coder, artist's camera pivot if the player is artist</returns>
    private Transform GetCameraPivotTransform()
    {
        if (pc.isCoder)
            return transform.Find("CoderCameraPivot").GetComponent<Transform>();
        
        //Works as else automatically
        return transform.Find("ArtistCameraPivot").GetComponent<Transform>();
    }
    
    /// <summary>
    /// <para>Makes player enter to easter egg state</para>
    /// </summary>
    private void EnterEasterEggState()
    {
        psd.currentState = PlayerStateData.PlayerState.EasterEggState;
        PositionCamera();
        
        //When player enter EasterEggState while running, it continues to move. It must stop
        rb.velocity = Vector3.zero;
    }

    /// <summary>
    /// <para>Makes player exit to normal state</para>
    /// </summary>
    private void ExitEasterEggState()
    {
        psd.currentState = PlayerStateData.PlayerState.NormalState;
        PositionCamera();
    }
    
    /// <summary>
    /// <para>Repositions camera according to player state</para>
    /// </summary>
    private void PositionCamera()
    {
        if (psd.currentState == PlayerStateData.PlayerState.NormalState)
            cameraPivot.DOLocalMoveY(defaultCameraPivotLocalYPosition, 1f);
        else if (psd.currentState == PlayerStateData.PlayerState.EasterEggState)
            cameraPivot.DOLocalMoveY(defaultCameraPivotLocalYPosition - 1f, 1f);
    }
}