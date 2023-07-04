using Cinemachine;
using DG.Tweening;
using UnityEngine;

/// <summary>
/// <para>Controls easter egg state</para>
/// </summary>
public class EasterEggController : MonoBehaviour
{
    public PlayerStateData psd;
    public CinemachineFreeLook cam;
    public PlayerController pc;
    
    public Rigidbody rb;
    public Transform cameraPivot;
    public float defaultCameraPivotLocalYPosition;

    private void Awake()
    {
        psd = GetComponent<PlayerStateData>();
        cam = GetComponent<CinemachineFreeLook>();
        pc = GetComponent<PlayerController>();

        rb = GetComponent<Rigidbody>();
        cameraPivot = transform.Find("CameraPivot").GetComponent<Transform>();
        defaultCameraPivotLocalYPosition = cameraPivot.localPosition.y;

        pc.OnEasterEggEnter += EnterEasterEggState;
        pc.OnEasterEggExit += ExitEasterEggState;
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