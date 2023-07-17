using UnityEngine;

public class SecondIslandBackgroundMusicManager : MonoBehaviour
{
    [Header("INFO NO TOUCH")]
    [SerializeField] private PlayerStateData artistPlayerStateData;
    [SerializeField] private AudioSource aus;
    [SerializeField] private float defaultVolume;
    
    private void Awake()
    {
        artistPlayerStateData = GameObject.Find("ArtistPlayer").GetComponent<PlayerStateData>();
        aus = GetComponent<AudioSource>();
        
        //starting volume assigned from inspector
        defaultVolume = aus.volume;
    }
    
    private void Update()
    {
        if (artistPlayerStateData.currentMainState == PlayerStateData.PlayerMainState.AbilityState) aus.volume = 0f;
        else aus.volume = defaultVolume;
    }
}