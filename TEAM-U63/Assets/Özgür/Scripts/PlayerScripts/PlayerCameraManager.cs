using Cinemachine;
using UnityEngine;

/// <summary>
/// <para>Updates main camera according to local player</para>
/// <para>Works for each player</para>
/// </summary>
public class PlayerCameraManager : MonoBehaviour
{
    private PlayerData pd;
    private CinemachineFreeLook cinemachineCamera;

    private void Awake()
    {
        pd = GetComponent<PlayerData>();
        cinemachineCamera = GetComponentInChildren<CinemachineFreeLook>();

        pd.OnControlSourceChanged += UpdateCurrentCamera;   //Needed for island 3 mechanics
        UpdateCurrentCamera();
    }

    private void UpdateCurrentCamera()
    {
        cinemachineCamera.enabled = pd.isLocal;
    }
}
