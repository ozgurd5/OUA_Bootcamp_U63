using System;
using Unity.Netcode;

/// <summary>
/// <para>Holds the data of if the player is host, client, controlled, remote, coder or artist</para>
/// <para>Works for each player, both in host and client sides</para>
/// </summary>
public class PlayerData : NetworkBehaviour
{
    private NetworkPlayerData npd;

    public event Action OnControlSourceChanged;
    
    /// <summary>
    /// <para>Controlled by this computer, or remote</para>
    /// </summary>
    public enum ControlSource
    {
        Local,
        Remote
    }
    
    public enum PlayerName
    {
        Coder,
        Artist
    }

    public ControlSource controlSource;
    public PlayerName playerName;

    private void Awake()
    {
        npd = NetworkPlayerData.Singleton;
        npd.OnIsHostCoderChanged += DecideControlSource;  //Needed for island 3 mechanics

        DecidePlayerName();
        DecideControlSource();
    }
    
    private void DecidePlayerName()
    {
        if (name == "CoderPlayer")
            playerName = PlayerName.Coder;
        else
            playerName = PlayerName.Artist;
    }

    private void DecideControlSource()
    {;
        if      (IsHost && npd.isHostCoder && playerName == PlayerName.Coder) controlSource = ControlSource.Local;
        else if (IsHost && !npd.isHostCoder && playerName == PlayerName.Artist) controlSource = ControlSource.Local;
        else if (!IsHost && !npd.isHostCoder && playerName == PlayerName.Coder) controlSource = ControlSource.Local;
        else if (!IsHost && npd.isHostCoder && playerName == PlayerName.Artist) controlSource = ControlSource.Local;
        else controlSource = ControlSource.Remote;
        
        OnControlSourceChanged?.Invoke();
    }
}