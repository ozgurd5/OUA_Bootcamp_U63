using Unity.Netcode;
using UnityEngine;

/// <summary>
/// <para>Handles synchronization of position with interpolation because Network Transform is bad at interpolation</para>
/// </summary>
public class NetworkSyncPosition : NetworkBehaviour
{
    private static float interpolationValue = 0.5f;
    
    private void Update()
    {
        if (!IsHost) return;
        
        //transform.position in this line is the position in the host side
        SyncClientPositionWithInterpolationClientRpc(transform.position);
    }
    
    /// <summary>
    /// <para>Sends position in the host to client and interpolates it in client side</para>
    /// <para>This method also provide host side interpolation because host is also a client</para>
    /// </summary>
    /// <param name="hostPosition">Position in the host side</param>
    [ClientRpc]
    private void SyncClientPositionWithInterpolationClientRpc(Vector3 hostPosition)
    {
        //transform.position in this line is the position in the client side + host side
        //So this method also provide host side interpolation because host is also a client
        transform.position = Vector3.Lerp(transform.position , hostPosition, interpolationValue);
    }
}