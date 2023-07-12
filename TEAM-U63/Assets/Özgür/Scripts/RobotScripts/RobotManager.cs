using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RobotManager : NetworkBehaviour
{
    //This should be static but we can't see static variables in inspector, therefore can't assign materials
    //We must manually assign materials by the order of RGB. It's already assigned and saved in the prefab but if a
    //..problem occur, especially about dividing zero, the problem may be about this list being empty
    [Header("Assign")]
    [SerializeField] private List<Material> robotMaterials;

    public enum RobotState
    {
        IsRouting = 0,
        IsSleeping = 1,
        IsHacked = 2,
        IsSleepingProcess = 3
    }

    public RobotState currentState = RobotState.IsRouting;

    public bool isLocal { get; private set; }
    public event Action OnLocalStatusChanged;
    public event Action OnRobotStateChanged;

    private PlayerData coderPlayerPd;
    private Rigidbody coderPlayerRb;
    private RobotController rc;
    private MeshRenderer mr;
    private Rigidbody rb;
    
    private int robotMaterialIndex;    //PlayerArtistPaintAbility.cs

    private void Awake()
    {
        rc = GetComponent<RobotController>();
        mr = transform.Find("BODY").GetComponent<MeshRenderer>();
        rb = GetComponent<Rigidbody>();

        coderPlayerPd = GameObject.Find("CoderPlayer").GetComponent<PlayerData>();
        
        //Robot must be local where the coder player is local
        coderPlayerPd.OnLocalStatusChanged += UpdateRobotLocalStatus;   //Needed for island 3 mechanics
        UpdateRobotLocalStatus();

        OnRobotStateChanged += HandleHackedState;
    }

    private void HandleHackedState()
    {
        //Robot can only move in it's own side while hacked
        if (currentState == RobotState.IsHacked && isLocal) rb.constraints = RigidbodyConstraints.None;
        else rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    /// <summary>
    /// <para>Robot must be local where the coder player is local</para>
    /// <para>Robot must not be physically interacted by the remote side</para>
    /// </summary>
    private void UpdateRobotLocalStatus()
    {
        isLocal = coderPlayerPd.isLocal;
        
        //Only needed in NetworkSyncPositionAndRotation.cs for not to reach coder player's player data in there :p
        OnLocalStatusChanged?.Invoke();
    }

    #region States

    /// <summary>
    /// <para>Changes the states of the robot and syncs it across the network</para>
    /// </summary>
    public void UpdateStates(int newState)
    {
        currentState = (RobotState)newState;
        OnRobotStateChanged?.Invoke();

        rc.enabled = newState == (int)RobotState.IsHacked;  //Do not sync this. Local only
            
        //isLocal in this line is the value in the host side because client can't call ClientRpc
        SyncIsGrabbedClientRpc(newState);
        
        //isLocal in this line must be the value in the client side and it is
        if (!IsHost) SyncIsGrabbedServerRpc(newState);
    }
    
    /// <summary>
    /// <para>Sends state values in the host to client</para>
    /// <para>Can't and must not work in host side</para>
    /// </summary>
    [ClientRpc]
    private void SyncIsGrabbedClientRpc(int newState)
    {
        //Since host is also a client, it will also try to run this method. It must not //TODO: what happens if it does?
        if (IsHost) return;
        
        currentState = (RobotState)newState;
        OnRobotStateChanged?.Invoke();
    }

    /// <summary>
    /// <para>Sends state values in the host to client</para>
    /// <para>Must not called by the host, be careful. Since host is also a client, it can call this method. If so,
    /// that would override client side states and cause object to not update it's states</para>
    /// </summary>
    [ServerRpc(RequireOwnership = false)]
    private void SyncIsGrabbedServerRpc(int newState)
    {
        currentState = (RobotState)newState;
        OnRobotStateChanged?.Invoke();
    }

    #endregion
    
    #region Painting

    /// <summary>
    /// <para>Paints cubes by increasing their material and tag</para>
    /// </summary>
    public void PaintRobot()
    {
        //Index will go like 1-2-3-1-2-3 on and on...
        robotMaterialIndex = (robotMaterialIndex + 1) % robotMaterials.Count;

        UpdateMaterialLocally();
        
        //robotMaterialIndex in this line are the states in the host side because client can't call ClientRpc
        UpdateCubeMaterialClientRpc(robotMaterialIndex);
        
        //robotMaterialIndex in this line must be states in the client side and it is
        if (!IsHost) UpdateCubeMaterialServerRpc(robotMaterialIndex);
    }

    private void UpdateMaterialLocally()
    {
        //Updating mesh renderer materials in Unity is ultra protected for several long reasons
        //Long story short: We can not change a single element of the mesh renderer's materials array
        //We can only change the complete array by assign an array to it
        //So we must make our changes in a temporary copy array and assign it to mesh renderer material
        
        Material[] newRobotMaterials = mr.materials;
        newRobotMaterials[0] = robotMaterials[robotMaterialIndex];
        mr.materials = newRobotMaterials;
    }

    /// <summary>
    /// <para>Sends position in the host to client and interpolates it in client side</para>
    /// <para>Can't and must not work in host side</para>
    /// <param name="newMaterialIndex">Material index in the host side</param>
    /// </summary>
    [ClientRpc]
    private void UpdateCubeMaterialClientRpc(int newMaterialIndex)
    {
        //Since host is also a client, it will also try to run this method. It must not //TODO: what happens if it does?
        if (IsHost) return;
        
        robotMaterialIndex = newMaterialIndex;
        UpdateMaterialLocally();
    }

    /// <summary>
    /// <para>Sends position in the client to host and interpolates it in host side</para>
    /// <para>Must not called by the host, be careful. Since host is also a client, it can call this method. If so,
    /// that would override client side index and cause object to not update it's index</para>
    /// <param name="newMaterialIndex">Material index in the client side</param>
    /// </summary>
    [ServerRpc(RequireOwnership = false)]
    private void UpdateCubeMaterialServerRpc(int newMaterialIndex)
    {
        robotMaterialIndex = newMaterialIndex;
        UpdateMaterialLocally();
    }

    #endregion
}