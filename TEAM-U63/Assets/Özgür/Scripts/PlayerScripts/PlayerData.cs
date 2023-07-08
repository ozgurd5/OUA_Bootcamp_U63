using System;
using Unity.Netcode;

/// <summary>
/// <para>Holds the data of if the player is host, client, controlled, remote, coder or artist</para>
/// <para>Works for each player both in host and client side</para>
/// </summary>
public class PlayerData : NetworkBehaviour
{
    private NetworkPlayerData npd;

    public event Action OnControlSourceChanged;
    
    /// <summary>
    /// <para>Host or client</para>
    /// </summary>
    public enum NetworkType
    {
        Host,
        Client
    }
    
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

    public NetworkType networkType;
    public ControlSource controlSource;
    public PlayerName playerName;

    private void Awake()
    {
        npd = NetworkPlayerData.Singleton;
        npd.OnIsHostCoderChanged += DecideControlSource;  //Needed for island 3 mechanics

        DecidePlayerName();
        DecideControlSource();
    }

    //TODO: this must be in Awake
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        DecideNetworkType();
    }

    private void DecideNetworkType()
    {
        if (IsHost)
            networkType = NetworkType.Host;
        else
            networkType = NetworkType.Client;
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