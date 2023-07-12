using Cinemachine;
using UnityEngine;

/// <summary>
/// <para>Updates main camera according to local player</para>
/// <para>Works for each player</para>
/// </summary>
public class PlayerCameraManager : MonoBehaviour
{
    private PlayerData pd;
    private PlayerStateData psd;
    private CinemachineFreeLook cinemachineCamera;

    private void Awake()
    {
        pd = GetComponent<PlayerData>();
        psd = GetComponent<PlayerStateData>();
        cinemachineCamera = GetComponentInChildren<CinemachineFreeLook>();

        PlayerQTEAbility.OnRobotHacked += UpdateCurrentCamera;
        pd.OnLocalStatusChanged += () => UpdateCurrentCamera(null);   //Needed for island 3 mechanics
        UpdateCurrentCamera(null);
    }

    //Needs rewriting for island 3 mechanics
    private void UpdateCurrentCamera(Transform tf)
    {
        if (psd.currentMainState != PlayerStateData.PlayerMainState.RobotControllingState)
            cinemachineCamera.enabled = pd.isLocal;

        else
        {
            cinemachineCamera.enabled = false;
            tf.GetComponentInChildren<CinemachineFreeLook>().enabled = true;
        }
    }
}
