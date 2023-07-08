using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

/// <summary>
/// <para>CREATES INCONSISTENT NETWORK ARCHITECTURE</para>
/// <para>Assign this scripts to both players - Works both host and client side</para>
/// <para>In this LAZY architecture, client's PlayerController.cs works in both sides. Host's script works only in host side.
/// Host's position and rotation sync must be handled by NetworkTransform.cs and NetworkPositionSync.cs</para>
/// <para>Client's animation works in both sides just as it's PlayerController.cs but host's animation works only in
/// host side just as it's own PlayerController.cs script. It's sync must be handled by NetworkLazyAnimationSync.cs</para>
/// <para>This script:</para>
/// <para>Input makes the client player move both sides, therefore position and rotation sync comes from host doesn't
/// needed and makes the player stutter in client side</para>
/// <para>This scripts close the sync scripts of the player controlled by client and enable it's rigidbody interpolation
/// for smooth movement</para>
/// </summary>
public class LazyNetworkDisableClientSync : NetworkBehaviour
{
    private NetworkPlayerData npd;
    private NetworkSyncPosition nsp;
    private NetworkTransform ntf;
    private Rigidbody rb;

    private void Awake()
    {
        npd = NetworkPlayerData.Singleton;
        nsp = GetComponent<NetworkSyncPosition>();
        ntf = GetComponent<NetworkTransform>();
        rb = GetComponent<Rigidbody>();

        npd.OnIsHostCoderChanged += UpdateSyncScripts;  //Needed for island 3 mechanics
        UpdateSyncScripts();
    }
    
    
    //TODO: BETTER CODE - FIND CURRENT PLAYER - FIND HOST PLAYER - FIND CLIENT PLAYER-
    private void UpdateSyncScripts()
    {
        if (npd.isHostCoder)
        {
            if (name == "ArtistPlayer")  //this is client
            {
                nsp.enabled = false;
                ntf.enabled = false;
                rb.interpolation = RigidbodyInterpolation.Interpolate;
            }

            else    //this is host
            {
                nsp.enabled = true;
                ntf.enabled = true;
                rb.interpolation = RigidbodyInterpolation.None;
            }
        }

        else
        {
            if (name == "CoderPlayer")  //this is client
            {
                nsp.enabled = false;
                ntf.enabled = false;
                rb.interpolation = RigidbodyInterpolation.Interpolate;
            }
            
            else    //this is host
            {
                nsp.enabled = true;
                ntf.enabled = true;
                rb.interpolation = RigidbodyInterpolation.None;
            }
        }
    }
}