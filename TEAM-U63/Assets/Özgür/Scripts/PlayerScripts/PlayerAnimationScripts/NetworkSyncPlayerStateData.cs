using Unity.Mathematics;
using Unity.Netcode;

/// <summary>
/// <para>Handles synchronization of player state data across the network for animation and sound sync</para>
/// <para>Works only for local player</para>
/// <para>Local objects transmit player state data, remote objects receive player state data</para>
/// </summary>
public class NetworkSyncPlayerStateData : NetworkBehaviour
{
    private PlayerData pd;
    private PlayerStateData psd;
    private PlayerAnimationManager pam;

    private float blendInterpolationValue = 0.5f;
    
    private void Awake()
    {
        pd = GetComponent<PlayerData>();
        psd = GetComponent<PlayerStateData>();
        pam = GetComponent<PlayerAnimationManager>();
    }
    
    private void Update()
    {
        if (!pd.isLocal) return;
        
        //States in this line are the states in the host side because client can't call ClientRpc
        SyncClientPlayerStateDataClientRpc((int)psd.currentMainState, psd.isIdle, psd.isWalking, psd.isRunning);
        SyncClientBlendValuesClientRpc(pam.idleAndMovingBlendValue, pam.walkingAndRunningBlendValue);
        
        //States in this line must be states in the client side and they are
        if (!IsHost)
        {
            SyncHostPlayerStateDataServerRpc((int)psd.currentMainState, psd.isIdle, psd.isWalking, psd.isRunning);
            SyncHostBlendValuesServerRpc(pam.idleAndMovingBlendValue, pam.walkingAndRunningBlendValue);
        }
    }

    /// <summary>
    /// <para>Sends position in the host to client and interpolates it in client side</para>
    /// <para>Can't and must not work in host side</para>
    /// </summary>
    [ClientRpc]
    private void SyncClientPlayerStateDataClientRpc(int newMainState, bool newIsIdle, bool newIsWalking, bool newIsRunning)
    {
        //Since host is also a client, it will also try to run this method. It must not //TODO: what happens if it does?
        if (IsHost) return;

        psd.currentMainState = (PlayerStateData.PlayerMainState)newMainState;
        psd.isIdle = newIsIdle;
        psd.isWalking = newIsWalking;
        psd.isRunning = newIsRunning;
    }

    /// <summary>
    /// <para>Sends position in the client to host and interpolates it in host side</para>
    /// <para>Must not called by the host, be careful. Since host is also a client, it can call this method. If so,
    /// that would override client side position and cause object to not update it's position</para>
    /// </summary>
    [ServerRpc(RequireOwnership = false)]
    private void SyncHostPlayerStateDataServerRpc(int newMainState, bool newIsIdle, bool newIsWalking, bool newIsRunning)
    {
        psd.currentMainState = (PlayerStateData.PlayerMainState)newMainState;
        psd.isIdle = newIsIdle;
        psd.isWalking = newIsWalking;
        psd.isRunning = newIsRunning;
    }
    
    /// <summary>
    /// <para>Sends position in the host to client and interpolates it in client side</para>
    /// <para>Can't and must not work in host side</para>
    /// </summary>
    [ClientRpc]
    private void SyncClientBlendValuesClientRpc(float newIdleAndMovingBlendValue, float newWalkingAndRunningBlendValue)
    {
        //Since host is also a client, it will also try to run this method. It must not //TODO: what happens if it does?
        if (IsHost) return;

        pam.idleAndMovingBlendValue = math.lerp(pam.idleAndMovingBlendValue, newIdleAndMovingBlendValue, blendInterpolationValue);
        pam.walkingAndRunningBlendValue = math.lerp(pam.walkingAndRunningBlendValue, newWalkingAndRunningBlendValue, blendInterpolationValue);
    }

    /// <summary>
    /// <para>Sends position in the client to host and interpolates it in host side</para>
    /// <para>Must not called by the host, be careful. Since host is also a client, it can call this method. If so,
    /// that would override client side position and cause object to not update it's position</para>
    /// </summary>
    [ServerRpc(RequireOwnership = false)]
    private void SyncHostBlendValuesServerRpc(float newIdleAndMovingBlendValue, float newWalkingAndRunningBlendValue)
    {
        pam.idleAndMovingBlendValue = math.lerp(pam.idleAndMovingBlendValue, newIdleAndMovingBlendValue, blendInterpolationValue);
        pam.walkingAndRunningBlendValue = math.lerp(pam.walkingAndRunningBlendValue, newWalkingAndRunningBlendValue, blendInterpolationValue);
    }
}
