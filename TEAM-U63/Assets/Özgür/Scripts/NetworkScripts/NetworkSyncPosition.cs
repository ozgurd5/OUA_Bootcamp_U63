using Unity.Netcode;
using UnityEngine;

/// <summary>
/// <para>Handles synchronization of position across the network with interpolation</para>
/// <para>Works for each object, both in host and client sides</para>
/// <para>Local objects transmit position data, remote objects receive position data</para>
/// </summary>
public class NetworkSyncPosition : NetworkBehaviour
{
    private static float interpolationValue = 0.5f;
    
    private bool isObjectLocal;
    public PlayerData pd;
    
    private Transform cameraTransform;
    private Vector3 lookingDirection;

    private void Awake()
    {
        if (CompareTag("Player"))
        {
            pd = GetComponent<PlayerData>();
            pd.OnControlSourceChanged += () => UpdateIsObjectLocal(pd.controlSource == PlayerData.ControlSource.Local);
            UpdateIsObjectLocal(pd.controlSource == PlayerData.ControlSource.Local);
            
            cameraTransform = Camera.main!.transform;
            
        }

        else
        {
            //Other objects should be host controlled by default
            UpdateIsObjectLocal(IsHost);
        }
    }
    
    public void UpdateIsObjectLocal(bool newIsObjectLocal)
    {
        isObjectLocal = newIsObjectLocal;
    }

    private void Update()
    {
        if (isObjectLocal)
        {
            //transform.position in this line is the position in the host side because client can't call ClientRpc
            SyncClientPositionWithInterpolationClientRpc(transform.position);
            
            //transform.position in this line is the position in the client side and it must be
            if (!IsHost) SyncHostPositionWithInterpolationServerRpc(transform.position);
        }
    }
    
    /// <summary>
    /// <para>Sends position in the host to client and interpolates it in client side</para>
    /// <para>Can't and must not work in host side</para>
    /// </summary>
    /// <param name="hostPosition">Position in the host side</param>
    [ClientRpc]
    private void SyncClientPositionWithInterpolationClientRpc(Vector3 hostPosition)
    {
        //Since host is also a client, it will also try to run this method. It must not //TODO: what happens if it does?
        if (IsHost) return;
        
        transform.position = Vector3.Lerp(transform.position , hostPosition, interpolationValue);
    }

    /// <summary>
    /// <para>Sends position in the client to host and interpolates it in host side</para>
    /// <para>Must not called by the host, be careful. Since host is also a client, it can call this method. If so,
    /// that would override client position and cause object to not update it's position</para>
    /// </summary>
    /// <param name="clientPosition">Position in the client side</param>
    [ServerRpc(RequireOwnership = false)]
    private void SyncHostPositionWithInterpolationServerRpc(Vector3 clientPosition)
    {
        transform.position = Vector3.Lerp(transform.position , clientPosition, interpolationValue);
    }
}