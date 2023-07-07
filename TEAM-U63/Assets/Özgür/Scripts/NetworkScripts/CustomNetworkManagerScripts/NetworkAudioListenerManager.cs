using Unity.Netcode;
using UnityEngine;

/// <summary>
/// <para>Responsible of arranging audio listeners according do controlled player</para>
/// <para>Single audio listener in main camera is actually enough but feels bad because cinemachine moves
/// the camera around the player and that makes the audio listener closer to audio sources even though player
/// is not in that distance</para>
/// <para>TLDR: 3D audio volume should increase or decrease according to player position, not camera position</para>
/// <para>Works both in host and client side</para>
/// </summary>
public class NetworkAudioListenerManager : NetworkBehaviour
{
    [Header("Assign")]
    [SerializeField] private AudioListener coderAudioListener;
    [SerializeField] private AudioListener artistAudioListener;

    private NetworkPlayerData npd;

    private void Awake()
    {
        npd = NetworkPlayerData.Singleton;

        npd.OnIsHostCoderChanged += UpdateAudioListeners;   //Needed for island 3 mechanics
        UpdateAudioListeners();
    }

    /// <summary>
    /// <para>Updates audio listeners according to controlled player</para>
    /// </summary>
    private void UpdateAudioListeners()
    {
        if (IsHost)
        {
            if (npd.isHostCoder)
            {
                coderAudioListener.enabled = true;
                artistAudioListener.enabled = false;
            }

            else
            {
                coderAudioListener.enabled = false;
                artistAudioListener.enabled = true;
            }
        }

        else
        {
            if (npd.isHostCoder)
            {
                coderAudioListener.enabled = false;
                artistAudioListener.enabled = true;
            }

            else
            {
                coderAudioListener.enabled = true;
                artistAudioListener.enabled = false;
            }
        }
    }
}
