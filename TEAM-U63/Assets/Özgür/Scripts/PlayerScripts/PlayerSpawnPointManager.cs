using UnityEngine;

/// <summary>
/// <para>Teleports players to the spawn points in the first frame of the game</para>
/// <para>Assign this script to the spawn point prefabs and select or not for isCoder</para>
/// </summary>
public class PlayerSpawnPointManager : MonoBehaviour
{
    [Header("SELECT FOR CODER")]
    [SerializeField] private bool isCoder;
    
    private GameObject coderPlayer;
    private GameObject artistPlayer;

    private void Start()
    {
        if (isCoder)
        {
            coderPlayer = GameObject.Find("CoderPlayer");
            coderPlayer.transform.position = transform.position;
        }
        
        else
        {
            artistPlayer = GameObject.Find("ArtistPlayer");
            artistPlayer.transform.position = transform.position;
        }
    }
}
