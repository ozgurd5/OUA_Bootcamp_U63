using Cinemachine;
using UnityEngine;

/// <summary>
/// <para>Updates main camera according to local player</para>
/// <para>Works for each player</para>
/// </summary>
public class PlayerCameraManager : MonoBehaviour
{
    private PlayerData pd;
    private CinemachineVirtualCamera cinemachineCamera;

    private void Awake()
    {
        pd = GetComponent<PlayerData>();
        cinemachineCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        
        pd.OnLocalStatusChanged += UpdateCurrentCamera;   //Needed for island 3 mechanics
    }
    
    private void UpdateCurrentCamera()
    {
        cinemachineCamera.enabled = pd.isLocal;
    }
}
