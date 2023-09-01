using System;
using Unity.Netcode;
using UnityEngine;

public class SpareCubes : NetworkBehaviour
{
    private GameObject spareCubesParent;

    private void Awake()
    {
        spareCubesParent = GameObject.Find("SPARECUBES");
        spareCubesParent.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V) && Input.GetKey(KeyCode.RightShift)) OpenGameObject();
    }

    private void OpenGameObject()
    {
        spareCubesParent.SetActive(true);
        
        SyncGravityForRemoteWhileGrabbingClientRpc();
        if (!IsHost) SyncGravityForRemoteWhileGrabbingServerRpc();
    }
    
    /// <summary>
    /// <para>Sends gravity status in the host to client</para>
    /// <para>Can't and must not work in host side</para>
    /// <param name="newUseGravity">Will the gravity enabled for the remote after this action?</param>
    /// </summary>
    [ClientRpc]
    private void SyncGravityForRemoteWhileGrabbingClientRpc()
    {
        //Since host is also a client, it will also try to run this method. It must not //TODO: what happens if it does?
        if (!IsHost) spareCubesParent.SetActive(true);
    }

    /// <summary>
    /// <para>Sends gravity status in the client to host</para>
    /// <para>Must not called by the host, be careful. Since host is also a client, it can call this method. If so,
    /// that would override client side local status and cause object to not update it's local status</para>
    /// <param name="newUseGravity">Will the gravity enabled for the remote after this action?</param>
    /// </summary>
    [ServerRpc(RequireOwnership = false)]
    private void SyncGravityForRemoteWhileGrabbingServerRpc()
    {
        spareCubesParent.SetActive(true);
    }
}
