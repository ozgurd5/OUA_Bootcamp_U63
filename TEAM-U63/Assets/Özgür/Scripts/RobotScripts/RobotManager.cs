using System;
using Unity.Netcode;
using UnityEngine;

public class RobotManager : NetworkBehaviour
{
    public bool isSleeping { get; private set; }    //PlayerQTEAbility.cs
    public bool isHacked { get; private set; }      //PlayerQTEAbility.cs
    
    //Sleeping process is necessary for robot to stand still while artist is singing
    //It can not be hacked during process. It can only be hacked while isSleeping is true
    public bool isSleepingProcess { get; private set; } //PlayerQTEAbility.cs
    
    public bool isLocal { get; private set; }   //This script
    public event Action OnLocalStatusChanged;
    public event Action OnRobotStateChanged;

    private PlayerData coderPlayerPd;
    private RobotController rc;

    private void Awake()
    {
        rc = GetComponent<RobotController>();

        coderPlayerPd = GameObject.Find("CoderPlayer").GetComponent<PlayerData>();
        
        //Robot must be local where the coder player is local
        coderPlayerPd.OnLocalStatusChanged += UpdateRobotLocalStatus;
        UpdateRobotLocalStatus();
    }

    /// <summary>
    /// <para>Robot must be local where the coder player is local</para>
    /// </summary>
    private void UpdateRobotLocalStatus()
    {
        isLocal = coderPlayerPd.isLocal;
        OnLocalStatusChanged?.Invoke();
    }
    
    #region States

    /// <summary>
    /// <para>Changes the states of the robot and syncs it across the network</para>
    /// </summary>
    public void UpdateStates(bool newIsSleeping, bool newIsHacked, bool newIsSleepingStatus)
    {
        isSleeping = newIsSleeping;
        isHacked = newIsHacked;
        isSleepingProcess = newIsSleepingStatus;
        OnRobotStateChanged?.Invoke();

        rc.enabled = isHacked;  //Do not sync this. Local only
            
        //isLocal in this line is the value in the host side because client can't call ClientRpc
        SyncIsGrabbedClientRpc(newIsSleeping, newIsHacked, newIsSleepingStatus);
        
        //isLocal in this line must be the value in the client side and it is
        if (!IsHost) SyncIsGrabbedServerRpc(newIsSleeping, newIsHacked, newIsSleepingStatus);
    }
    
    /// <summary>
    /// <para>Sends state values in the host to client</para>
    /// <para>Can't and must not work in host side</para>
    /// </summary>
    [ClientRpc]
    private void SyncIsGrabbedClientRpc(bool newIsSleeping, bool newIsHacked, bool newIsSleepingStatus)
    {
        //Since host is also a client, it will also try to run this method. It must not //TODO: what happens if it does?
        if (IsHost) return;
        
        isSleeping = newIsSleeping;
        isHacked = newIsHacked;
        isSleepingProcess = newIsSleepingStatus;
        OnRobotStateChanged?.Invoke();
    }

    /// <summary>
    /// <para>Sends state values in the host to client</para>
    /// <para>Must not called by the host, be careful. Since host is also a client, it can call this method. If so,
    /// that would override client side states and cause object to not update it's states</para>
    /// </summary>
    [ServerRpc(RequireOwnership = false)]
    private void SyncIsGrabbedServerRpc(bool newIsSleeping, bool newIsHacked, bool newIsSleepingStatus)
    {
        isSleeping = newIsSleeping;
        isHacked = newIsHacked;
        isSleepingProcess = newIsSleepingStatus;
        OnRobotStateChanged?.Invoke();
    }

    #endregion
}