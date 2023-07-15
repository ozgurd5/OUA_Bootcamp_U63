using UnityEngine;

/// <summary>
/// <para>Responsible of sound management of the player</para>
/// <para>Works for each player, both in host and client sides</para>
/// <para>Audio source must work in each player but audio listener must only work in local player</para>
/// </summary>
public class PlayerSoundManager : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private AudioClip walkingSound;
    [SerializeField] private AudioClip runningSound;
    
    [Header("Must be between 0 and 1 - Range is buggy")]
    [SerializeField] private float walkingSoundVolume = 1f;
    [SerializeField] private float runningSoundVolume = 1f;

    private PlayerData pd;
    private PlayerStateData psd;
    private AudioListener al;
    private AudioSource aus;

    private void Awake()
    {
        pd = GetComponent<PlayerData>();
        psd = GetComponent<PlayerStateData>();
        aus = GetComponent<AudioSource>();
        al = GetComponent<AudioListener>();

        pd.OnLocalStatusChanged += UpdateAudioListener;   //Needed for island 3 mechanics
        UpdateAudioListener();
    }

    private void Update()
    {
        if (psd.isMoving)
        {
            SelectClip();
            PlaySound();
        }
        
        else
        {
            //Setting loop property to false feels more natural then using aus.Stop();
            aus.loop = false;
        }
    }

    /// <summary>
    /// <para>Selects clip according to moving or walking</para>
    /// </summary>
    private void SelectClip()
    {
        if (psd.isWalking)
        {
            aus.volume = walkingSoundVolume;
            aus.clip = walkingSound;
        }
        
        else if (psd.isRunning)
        {
            aus.volume = runningSoundVolume;
            aus.clip = runningSound;
        }
    }

    /// <summary>
    /// <para>Starts playing the selected clip</para>
    /// </summary>
    private void PlaySound()
    {
        if (aus.isPlaying) return;
        
        //Making loop property to false feels more natural then using Stop method, but we have to enable it everytime
        aus.loop = true;
        aus.Play();
    }

    /// <summary>
    /// <para>3D audio volume should increase or decrease according to player position, not camera position. Therefore we
    /// must not place the audio listener to main camera because it's moving and rotating around the player. Each player
    /// has an audio listener and we have to only enable the local player's audio listener</para>
    /// </summary>
    private void UpdateAudioListener()
    {
        al.enabled = pd.isLocal;
    }
}
