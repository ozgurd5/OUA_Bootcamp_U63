using System;
using Unity.Netcode;

/// <summary>
/// <para>Holds the data of if the player is host, client, controlled, remote, coder or artist</para>
/// <para>Works for each player</para>
/// </summary>
public class PlayerData : NetworkBehaviour
{
    private NetworkPlayerData npd;

    public event Action OnControlSourceChanged;
    
    public bool isLocal;

    private void Awake()
    {
        npd = NetworkPlayerData.Singleton;
        npd.OnIsHostCoderChanged += DecideControlSource;  //Needed for island 3 mechanics

        DecideControlSource();
    }

    private void DecideControlSource()
    {;
        if      (IsHost && npd.isHostCoder && name == "CoderPlayer") isLocal = true;
        else if (IsHost && !npd.isHostCoder && name == "ArtistPlayer") isLocal = true;
        else if (!IsHost && !npd.isHostCoder && name == "CoderPlayer") isLocal = true;
        else if (!IsHost && npd.isHostCoder && name == "ArtistPlayer") isLocal = true;
        else isLocal = false;
        
        OnControlSourceChanged?.Invoke();
    }
}