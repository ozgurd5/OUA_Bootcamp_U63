using Cinemachine;
using UnityEngine;

/// <summary>
/// <para>Updates main camera according to local player</para>
/// <para>Works for each player</para>
/// </summary>
public class PlayerCameraManager : MonoBehaviour
{
    [Header("Info - No Touch")]
    public Transform cameraTransform;
    public CinemachineVirtualCamera cam;
    
    private PlayerData pd;

    private void Awake()
    {
        pd = GetComponent<PlayerData>();

        if (name == "CoderPlayer") cameraTransform = GameObject.Find("CoderCamera").transform;
        else cameraTransform = GameObject.Find("ArtistCamera").transform;
        
        cam = cameraTransform.GetComponent<CinemachineVirtualCamera>();
        
        pd.OnLocalStatusChanged += UpdateCurrentCamera;   //Needed for island 3 mechanics
    }

    private void UpdateCurrentCamera()
    {
        cam.enabled = pd.isLocal;
    }
}
