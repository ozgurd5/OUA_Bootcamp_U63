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
    //There should be a way to do it but don't know, don't care
    //Long story short, we must manually assign materials by the order of RGB to every cube
    [Header("Assign")]
    [SerializeField] private List<Material> puzzleMaterials;
    private string[] puzzlesTag = { "RedPuzzle", "GreenPuzzle", "BluePuzzle" };

    private MeshRenderer mr;
    
    public bool isGrabbed;          //PlayerGrabbing.cs
    public int materialAndTagIndex; //PlayerArtistPaintAbility.cs
    
    public bool isLocal;
    public event Action OnLocalStatusChanged;

    private void Awake()
    {
        mr = GetComponent<MeshRenderer>();
    }

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

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Debug.Log(col.gameObject.name);
            isLocal = col.gameObject.GetComponent<PlayerData>().isLocal;
            Debug.Log("is local " + isLocal);
            OnLocalStatusChanged?.Invoke();
            
            //isLocal in this line is the value in the host side because client can't call ClientRpc
            SyncCubeIsLocalClientRpc(isLocal);
        
            //isLocal in this line must be the value in the client side and it is
            if (!IsHost) SyncCubeIsLocalServerRpc(isLocal);
        }
    }
    
    /// <summary>
    /// <para>Sends local status in the host to client. They must be reverse of themselves</para>
    /// <para>Can't and must not work in host side</para>
    /// <param name="newIsLocal">Will the cube become local after this action?</param>
    /// </summary>
    [ClientRpc]
    private void SyncCubeIsLocalClientRpc(bool newIsLocal)
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
    private void SyncCubeIsLocalServerRpc(bool newIsLocal)
    {
        //isLocal value will be assigned twice with the same value twice:
        //1- local player will assign it in the OnTriggerEnter 2- remote player assign it in this rpc
        //No need to assign the variable with the same value twice and of course invoke the event twice
        if (isLocal == !newIsLocal) return;
        
        isLocal = !newIsLocal;
        OnLocalStatusChanged?.Invoke();
    }
}