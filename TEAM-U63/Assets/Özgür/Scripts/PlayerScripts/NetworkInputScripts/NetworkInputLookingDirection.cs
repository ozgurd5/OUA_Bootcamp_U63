using Unity.Netcode;
using UnityEngine;

/// <summary>
/// <para>Calculates looking directions, sends them to NetworkInputManager and syncs them across the network</para>
/// <para>Works both in host and client side</para>
/// </summary>
public class NetworkInputLookingDirection : NetworkBehaviour
{
    private NetworkInputManager nim;
    private Transform cameraTransform;
    
    private Vector3 lookingDirection;
    
    private void Awake()
    {
        nim = NetworkInputManager.Singleton;
        cameraTransform = Camera.main!.transform;
    }
    
    //Methods that depend lookingDirection in PlayerController.cs are working in FixedUpdate, so we can calculate..
    //..and sync lookingDirection in FixedUpdate
    private void FixedUpdate()
    {
        lookingDirection = cameraTransform.forward;
        lookingDirection.y = 0f;

        if (IsHost)
            nim.hostInput.lookingDirection = lookingDirection;
        else
            nim.clientInput.lookingDirection = lookingDirection;

        if (IsHost) return;
        SendLookingDirectionToHostServerRpc(lookingDirection);
    }
    
    /// <summary>
    /// <para>Sends looking direction from the client side to host side</para>
    /// <para>Must not be called from the host side. Since host is also a client, it can call this method, be careful</para>
    /// <param name="clientLookingDirection">Looking direction from client side that will be send to host side</param>
    /// </summary>
    [ServerRpc]
    private void SendLookingDirectionToHostServerRpc(Vector3 clientLookingDirection)
    {
        nim.clientInput.lookingDirection = clientLookingDirection;
    }
}
