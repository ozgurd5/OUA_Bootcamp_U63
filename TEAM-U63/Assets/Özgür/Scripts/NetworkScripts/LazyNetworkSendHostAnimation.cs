using System;
using Unity.Netcode;
using UnityEngine;

/// <summary>
/// <para>CREATES INCONSISTENT NETWORK ARCHITECTURE</para>
/// <para>Assign this scripts to both players - Works both host and client side</para>
/// <para>In this LAZY architecture, client's PlayerController.cs works in both sides. Host's script works only in host side.
/// Host's position and rotation sync must be handled by NetworkTransform.cs and NetworkPositionSync.cs</para>
/// <para>Client's animation works in both sides just as it's PlayerController.cs but host's animation works only in
/// host side just as it's own PlayerController.cs script. It's sync must be handled by NetworkLazyAnimationSync.cs</para>
/// <para>This script:</para>
/// <para>Host animation doesn't play in the client side because client doesn't get the host input and run it's logic
/// in it's own side. Host do that. Host get client's input and run it's logic. Lazy architecture doesn't prevent client
/// to run it's own logic. Anyway. We must send host's animation data to client so client can run host's animations</para>
/// </summary>
public class LazyNetworkSendHostAnimation : NetworkBehaviour
{
    private PlayerStateData psd;
    
    public bool networkIsIdle;
    public bool networkIsWalking;
    public bool networkIsRunning;

    private void Awake()
    {
        psd = GetComponent<PlayerStateData>();
    }

    private void Update()
    {
        GatherStates();
        SendStatesClientRpc(networkIsIdle, networkIsWalking, networkIsRunning);
    }

    private void GatherStates()
    {
        if (!IsHost) return;
        Debug.Log("host animations gathered. HOST");
        networkIsIdle = psd.isIdle;
        networkIsWalking = psd.isWalking;
        networkIsRunning = psd.isRunning;
    }

    [ClientRpc]
    private void SendStatesClientRpc(bool newIsIdle, bool newIsWalking, bool newIsRunning)
    {
        if (IsHost) return;
        Debug.Log("host animations sent. CLIENT");
        networkIsIdle = newIsIdle;
        networkIsWalking = newIsWalking;
        networkIsRunning = newIsRunning;
    }
}
