using Unity.Netcode;
using UnityEngine;

public class NetworkSync : NetworkBehaviour
{
    private static float interpolationValue = 0.5f;
    
    private void Update()
    {
        if (!IsHost) return;
        //transform.position in this line is the position in the host side
        SyncClientPositionWithInterpolationClientRpc(transform.position);
    }
    
    /// <summary>
    /// <para>Sends position in the host side to client and interpolates it in client side</para>
    /// </summary>
    /// <param name="hostPosition">Position in the host side</param>
    [ClientRpc]
    public void SyncClientPositionWithInterpolationClientRpc(Vector3 hostPosition)
    {
        //transform.position in this line is the position in the client side
        transform.position = Vector3.Lerp(transform.position , hostPosition, interpolationValue);
    }
}