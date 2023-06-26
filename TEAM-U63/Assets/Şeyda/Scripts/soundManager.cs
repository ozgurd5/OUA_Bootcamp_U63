using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundManager : MonoBehaviour
{
    public static soundManager Instance;

    [SerializeField] private AudioSource buttonSoundSource, glitchSoundSource, windSoundSource; 

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

        }

        else
        {
            Destroy(gameObject);
        }
    }
 
    public void playSound(AudioClip clip)
    {
        buttonSoundSource.PlayOneShot(clip);
    }
}
