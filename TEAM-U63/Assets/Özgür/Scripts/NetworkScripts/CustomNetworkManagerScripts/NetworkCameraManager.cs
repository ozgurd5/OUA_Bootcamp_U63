using Cinemachine;
using Unity.Netcode;
using UnityEngine;

/// <summary>
/// <para>Makes the cinemachine camera of the controlled player main camera by changing it's priority</para>
/// <para>Works both in host and client side</para>
/// </summary>
public class NetworkCameraManager : NetworkBehaviour
{
    [Header("Assign")]
    [SerializeField] private CinemachineFreeLook coderCamera;
    [SerializeField] private CinemachineFreeLook artistCamera;
    
    private NetworkPlayerData npd;
    
    private void Awake()
    {
        npd = NetworkPlayerData.Singleton;

        npd.OnIsHostCoderChanged += UpdateCameraPriorities; //Needed for island 3 mechanics
        UpdateCameraPriorities();
    }
    
    /// <summary>
    /// <para>Update the main cinemachine camera according to controlled player by changing it's priority</para>
    /// </summary>
    private void UpdateCameraPriorities()
    {
        if (IsHost)
        {
            if (npd.isHostCoder)
                SelectCoderCamera();
            else
                SelectArtistCamera();
        }

        else
        {
            if (npd.isHostCoder)
                SelectArtistCamera();
            else
                SelectCoderCamera();
        }
    }

    /// <summary>
    /// <para>Makes coder's camera active camera by changing it's priority</para>
    /// </summary>
    private void SelectCoderCamera()
    {
        coderCamera.Priority = 20;
        artistCamera.Priority = 10;
    }
    
    
    /// <summary>
    /// <para>Makes artist's camera active camera by changing it's priority</para>
    /// </summary>
    private void SelectArtistCamera()
    {
        coderCamera.Priority = 10;
        artistCamera.Priority = 20;
    }
}
