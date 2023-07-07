using UnityEngine;

/// <summary>
/// <para>Responsible of sound management of the player</para>
/// </summary>
public class PlayerSoundManager : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private AudioClip walkingSound;
    [SerializeField] private AudioClip runningSound;
    
    private PlayerStateData psd;
    private AudioSource aus;

    private void Awake()
    {
        psd = GetComponent<PlayerStateData>();
        aus = GetComponent<AudioSource>();
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
            aus.volume = 0.3f;  //Walking sound is too loud
            aus.clip = walkingSound;
        }
        
        else if (psd.isRunning)
        {
            aus.volume = 1f;
            aus.clip = runningSound;
        }
    }

    /// <summary>
    /// <para>Starts playing the selected sound</para>
    /// </summary>
    private void PlaySound()
    {
        if (aus.isPlaying) return;
        
        //Making loop false feels more natural then using Stop method, but we have to enable it everytime
        aus.loop = true;
        aus.Play();
    }
}
