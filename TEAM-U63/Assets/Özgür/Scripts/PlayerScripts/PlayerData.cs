using System;
using Unity.Netcode;
using UnityEngine;

/// <summary>
/// <para>Holds the data of if the player is host, client, controlled, remote, coder or artist</para>
/// <para>Works for each player</para>
/// </summary>
public class PlayerData : NetworkBehaviour
{
    private NetworkPlayerData npd;

    public event Action OnLocalStatusChanged;
    
    public bool isLocal;

    private void Awake()
    {
        npd = NetworkPlayerData.Singleton;
    }
    
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        DecideControlSource();
    }

    //It's public for TestScript.cs
    public void DecideControlSource()
    {
        if (IsHost && npd.isHostCoder && name == "CoderPlayer") isLocal = true;
        else if (IsHost && !npd.isHostCoder && name == "ArtistPlayer") isLocal = true;
        else if (!IsHost && !npd.isHostCoder && name == "CoderPlayer") isLocal = true;
        else if (!IsHost && npd.isHostCoder && name == "ArtistPlayer") isLocal = true;
        else isLocal = false;
        
        OnLocalStatusChanged?.Invoke();
    }
}