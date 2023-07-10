using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

/// <summary>
/// <para>Responsible of cube's material, tag, local status and their sync across the network</para>
/// </summary>
public class CubeManager : NetworkBehaviour
{
    //This should be static but we can't see static variables in inspector, therefore can't assign materials
    //We must manually assign materials by the order of RGB. It's already assigned and saved in the prefab but if a
    //..problem occur, especially about dividing zero, the problem may be about this list being empty
    [Header("Assign")]
    [SerializeField] private List<Material> puzzleMaterials;
    private string[] puzzlesTag = { "RedPuzzle", "GreenPuzzle", "BluePuzzle" };

    private MeshRenderer mr;
    private Rigidbody rb;
    
    private int materialAndTagIndex;    //PlayerArtistPaintAbility.cs
    
    public bool isGrabbed { get; private set; } //PlayerGrabbing.cs
    public bool isLocal { get; private set; }   //This script //PlayerGrabbing.cs
    public event Action OnLocalStatusChanged;

    private void Awake()
    {
        mr = GetComponent<MeshRenderer>();
        rb = GetComponent<Rigidbody>();
    }

    #region Parenting

    public void UpdateParentUsingNetworkParentListID(int networkParentListID)
    {
        if (IsHost) transform.parent = NetworkParentingManager.Singleton.FindTransformUsingID(networkParentListID);
        else UpdateParentUsingNetworkParentListIDServerRpc(networkParentListID);
    }

    [ServerRpc(RequireOwnership = false)]
    private void UpdateParentUsingNetworkParentListIDServerRpc(int networkParentListID)
    {
        transform.parent = NetworkParentingManager.Singleton.FindTransformUsingID(networkParentListID);
    }

    #endregion

    #region Gravity

    /// <summary>
    /// <para>Enables and disables the gravity of the cube and syncs it across the network</para>
    /// <param name="newUseGravity">Will the gravity enabled for the remote after this action?</param>
    /// </summary>
    public void UpdateGravity(bool newUseGravity)
    {
        rb.useGravity = newUseGravity;
        
        SyncGravityForRemoteWhileGrabbingClientRpc(newUseGravity);
        if (!IsHost) SyncGravityForRemoteWhileGrabbingServerRpc(newUseGravity);
    }
    
    /// <summary>
    /// <para>Sends gravity status in the host to client</para>
    /// <para>Can't and must not work in host side</para>
    /// <param name="newUseGravity">Will the gravity enabled for the remote after this action?</param>
    /// </summary>
    [ClientRpc]
    private void SyncGravityForRemoteWhileGrabbingClientRpc(bool newUseGravity)
    {
        //Since host is also a client, it will also try to run this method. It must not //TODO: what happens if it does?
        if (!IsHost) rb.useGravity = newUseGravity;
    }

    /// <summary>
    /// <para>Sends gravity status in the client to host</para>
    /// <para>Must not called by the host, be careful. Since host is also a client, it can call this method. If so,
    /// that would override client side local status and cause object to not update it's local status</para>
    /// <param name="newUseGravity">Will the gravity enabled for the remote after this action?</param>
    /// </summary>
    [ServerRpc(RequireOwnership = false)]
    private void SyncGravityForRemoteWhileGrabbingServerRpc(bool newUseGravity)
    {
        rb.useGravity = newUseGravity;
    }

    #endregion

    #region IsGrabbed

    /// <summary>
    /// <para>Changes the isGrabbed state of the cube and syncs it across the network</para>
    /// </summary>
    /// <param name="newIsGrabbed">Will the cube become grabbed after this action?</param>
    public void UpdateIsGrabbed(bool newIsGrabbed)
    {
        isGrabbed = newIsGrabbed;
            
        //isLocal in this line is the value in the host side because client can't call ClientRpc
        SyncIsGrabbedClientRpc(newIsGrabbed);
        
        //isLocal in this line must be the value in the client side and it is
        if (!IsHost) SyncIsGrabbedServerRpc(newIsGrabbed);
    }
    
    /// <summary>
    /// <para>Sends isGrabbed value in the host to client</para>
    /// <para>Can't and must not work in host side</para>
    /// <param name="newIsGrabbed">Will the cube become local after this action?</param>
    /// </summary>
    [ClientRpc]
    private void SyncIsGrabbedClientRpc(bool newIsGrabbed)
    {
        //Since host is also a client, it will also try to run this method. It must not //TODO: what happens if it does?
        if (IsHost) return;
        
        isGrabbed = newIsGrabbed;
    }

    /// <summary>
    /// <para>Sends isGrabbed value in the host to client</para>
    /// <para>Must not called by the host, be careful. Since host is also a client, it can call this method. If so,
    /// that would override client side local status and cause object to not update it's local status</para>
    /// <param name="newIsGrabbed">Will the cube become local after this action?</param>
    /// </summary>
    [ServerRpc(RequireOwnership = false)]
    private void SyncIsGrabbedServerRpc(bool newIsGrabbed)
    {
        isGrabbed = newIsGrabbed;
    }

    #endregion
    
    #region Painting

    /// <summary>
    /// <para>Paints cubes by increasing their material and tag</para>
    /// </summary>
    public void PaintCube()
    {
        //Index will go like 1-2-3-1-2-3 on and on...
        materialAndTagIndex = (materialAndTagIndex + 1) % puzzleMaterials.Count;
        
        UpdateMaterialAndTagLocally();
        
        //materialAndTagIndex in this line are the states in the host side because client can't call ClientRpc
        UpdateCubeMaterialAndTagClientRpc(materialAndTagIndex);
        
        //materialAndTagIndex in this line must be states in the client side and it is
        if (!IsHost) UpdateCubeMaterialAndTagServerRpc(materialAndTagIndex);
    }

    private void UpdateMaterialAndTagLocally()
    {
        tag = puzzlesTag[materialAndTagIndex];
        
        //Updating mesh renderer materials in Unity is ultra protected for several long reasons
        //Long story short: We can not change a single element of the mesh renderer's materials array
        //We can only change the complete array by assign an array to it
        //So we must make our changes in a temporary copy array and assign it to mesh renderer material
        
        Material[] newCubeMaterials = mr.materials;
        newCubeMaterials[0] = puzzleMaterials[materialAndTagIndex];
        mr.materials = newCubeMaterials;
    }

    /// <summary>
    /// <para>Sends position in the host to client and interpolates it in client side</para>
    /// <para>Can't and must not work in host side</para>
    /// <param name="newMaterialAndTagIndex">Material and tag index in the host side</param>
    /// </summary>
    [ClientRpc]
    private void UpdateCubeMaterialAndTagClientRpc(int newMaterialAndTagIndex)
    {
        //Since host is also a client, it will also try to run this method. It must not //TODO: what happens if it does?
        if (IsHost) return;
        
        materialAndTagIndex = newMaterialAndTagIndex;
        UpdateMaterialAndTagLocally();
    }

    /// <summary>
    /// <para>Sends position in the client to host and interpolates it in host side</para>
    /// <para>Must not called by the host, be careful. Since host is also a client, it can call this method. If so,
    /// that would override client side index and cause object to not update it's index</para>
    /// <param name="newMaterialAndTagIndex">Material and tag index in the client side</param>
    /// </summary>
    [ServerRpc(RequireOwnership = false)]
    private void UpdateCubeMaterialAndTagServerRpc(int newMaterialAndTagIndex)
    {
        materialAndTagIndex = newMaterialAndTagIndex;
        UpdateMaterialAndTagLocally();
    }

    #endregion

    #region LocalStatus

    private void OnCollisionEnter(Collision col)
    {
        if (isGrabbed) return; //Must not change local status if grabbed. Player who grab the cube is the owner of it
        if (!col.gameObject.CompareTag("Player")) return;
        
        ChangeCubeLocalStatus(col.gameObject.GetComponent<PlayerData>().isLocal);
    }
    
    /// <summary>
    /// <para>Changes the local status of the cube and syncs it across the network</para>
    /// </summary>
    /// <param name="newIsLocal">Will the cube become local after this action?</param>
    public void ChangeCubeLocalStatus(bool newIsLocal)
    {
        isLocal = newIsLocal;
        OnLocalStatusChanged?.Invoke();
            
        //isLocal in this line is the value in the host side because client can't call ClientRpc
        SyncCubeLocalStatusClientRpc(newIsLocal);
        
        //isLocal in this line must be the value in the client side and it is
        if (!IsHost) SyncCubeLocalStatusServerRpc(newIsLocal);
    }
    
    /// <summary>
    /// <para>Sends local status in the host to client. They must be reverse of themselves</para>
    /// <para>Can't and must not work in host side</para>
    /// <param name="newIsLocal">Will the cube become local after this action?</param>
    /// </summary>
    [ClientRpc]
    private void SyncCubeLocalStatusClientRpc(bool newIsLocal)
    {
        //Since host is also a client, it will also try to run this method. It must not //TODO: what happens if it does?
        if (IsHost) return;
        
        //isLocal value will be assigned twice with the same value twice:
        //1- local player will assign it in the OnTriggerEnter 2- remote player assign it in this rpc
        //No need to assign the variable with the same value twice and of course invoke the event twice
        if (isLocal == !newIsLocal) return;
        
        isLocal = !newIsLocal;
        OnLocalStatusChanged?.Invoke();
    }

    /// <summary>
    /// <para>Sends local status in the host to client. They must be reverse of themselves</para>
    /// <para>Must not called by the host, be careful. Since host is also a client, it can call this method. If so,
    /// that would override client side local status and cause object to not update it's local status</para>
    /// <param name="newIsLocal">Will the cube become local after this action?</param>
    /// </summary>
    [ServerRpc(RequireOwnership = false)]
    private void SyncCubeLocalStatusServerRpc(bool newIsLocal)
    {
        //isLocal value will be assigned twice with the same value twice:
        //1- local player will assign it in the OnTriggerEnter 2- remote player assign it in this rpc
        //No need to assign the variable with the same value twice and of course invoke the event twice
        if (isLocal == !newIsLocal) return;
        
        isLocal = !newIsLocal;
        OnLocalStatusChanged?.Invoke();
    }

    #endregion
}