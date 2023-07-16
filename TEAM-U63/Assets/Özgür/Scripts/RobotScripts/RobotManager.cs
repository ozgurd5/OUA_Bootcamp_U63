using System;
using System.Collections.Generic;
using Cinemachine;
using Unity.Netcode;
using UnityEngine;

public class RobotManager : NetworkBehaviour
{
    public static RobotManager currentControlledRobot;
    public static event Action OnCurrentControlledRobotChanged;
    
    //This should be static but we can't see static variables in inspector, therefore can't assign materials
    //We must manually assign materials by the order of RGB. It's already assigned and saved in the prefab but if a
    //..problem occur, especially about dividing zero, the problem may be about this list being empty
    [Header("Assign")]
    [SerializeField] private List<Material> robotMaterials;

    public enum RobotState
    {
        Routing = 0,
        Sleeping = 1,
        Hacked = 2,
        SleepingProcess = 3,
    }

    public RobotState currentState { get; private set; }

    public bool isLocal { get; private set; }
    public event Action OnLocalStatusChanged;
    public event Action OnRobotStateChanged;
    public event Action<int> OnRobotPainted;

    private PlayerData coderPlayerPd;
    private PlayerInputManager coderPlayerPim;
    
    private RobotController rc;
    private MeshRenderer mr;
    private Rigidbody rb;
    public CinemachineFreeLook cam;
    
    private int robotMaterialIndex;  //PlayerArtistPaintAbility.cs

    private void Awake()
    {
        currentState = RobotState.Routing;
        
        coderPlayerPd = GameObject.Find("CoderPlayer").GetComponent<PlayerData>();
        coderPlayerPim = coderPlayerPd.GetComponent<PlayerInputManager>();
        
        rc = GetComponent<RobotController>();
        mr = transform.Find("BODY").GetComponent<MeshRenderer>();
        rb = GetComponent<Rigidbody>();
        cam = GetComponentInChildren<CinemachineFreeLook>();

        //Robot must be local where the coder player is local
        coderPlayerPd.OnLocalStatusChanged += UpdateRobotLocalStatus;   //Needed for island 3 mechanics
        UpdateRobotLocalStatus();

        OnRobotStateChanged += HandleHackedStateTransition;
    }

    private void Update()
    {
        //Responsibility chart of the states/rigidbody/camera
        //1.a Robot - Hacked Enter - PlayerQTEAbility.cs (and RobotManager.cs)
        //1.b Player - RobotControllingState Enter - PlayerQTEAbility.cs
        //2.a Robot - Hacked Exit to Sleeping - RobotManager.cs
        //2.b Player - RobotControllingState Exit to NormalState - PlayerController.cs
        
        //2.a
        if (coderPlayerPim.isPrimaryAbilityKeyDown && currentState == RobotState.Hacked && isLocal)
            UpdateRobotState((int)RobotState.Sleeping);
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
    public void UpdateRobotState(int newState)
    {
        currentState = (RobotState)newState;
        OnRobotStateChanged?.Invoke();

        rc.enabled = newState == (int)RobotState.Hacked;  //Do not sync this. Local only
            
        //isLocal in this line is the value in the host side because client can't call ClientRpc
        SyncRobotStateClientRpc(newState);
        
        //isLocal in this line must be the value in the client side and it is
        if (!IsHost) SyncRobotStateServerRpc(newState);
    }
    
    /// <summary>
    /// <para>Sends state values in the host to client</para>
    /// <para>Can't and must not work in host side</para>
    /// </summary>
    [ClientRpc]
    private void SyncRobotStateClientRpc(int newState)
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
    private void SyncRobotStateServerRpc(int newState)
    {
        currentState = (RobotState)newState;
        OnRobotStateChanged?.Invoke();
    }    
    
    /// <summary>
    /// <para>Handles the camera and rigidbody according to entering or exiting Hacked state</para>
    /// <para>Must only be called in UpdateRobotState method, do not call anywhere else</para>
    /// </summary>
    private void HandleHackedStateTransition()
    {
        if (!isLocal)
        {
            //Robot can only move in it's own side
            rb.constraints = RigidbodyConstraints.FreezeAll;
            return;
        }
        
        //Enter hacking state in the local side
        if (currentState == RobotState.Hacked)
        {
            //Robot can only move in it's own side when hacked
            rb.constraints = RigidbodyConstraints.None;
            cam.enabled = true;
            
            currentControlledRobot = this;
            OnCurrentControlledRobotChanged?.Invoke();
        }
        
        //Exit hacking state in the local side
        else
        {
            //Robot can only move in it's own side when hacked
            rb.constraints = RigidbodyConstraints.FreezeAll;
            cam.enabled = false;
            currentControlledRobot = this;
        }
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
        OnRobotPainted?.Invoke(robotMaterialIndex);
        
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
        OnRobotPainted?.Invoke(newMaterialIndex);
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
        OnRobotPainted?.Invoke(newMaterialIndex);
    }

    #endregion
}