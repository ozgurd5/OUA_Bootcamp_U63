using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

/// <summary>
/// <para>CREATES INCONSISTENT NETWORK ARCHITECTURE</para>
/// <para>Assign this scripts to both players - Works both host and client side</para>
/// <para>In this architecture, client moves and animates both in host and client side</para>
/// <para>Input makes the client player move both sides, therefore position and rotation sync comes from host deosn't
/// needed and makes the player stutter in client side</para>
/// <para>This scripts close the sync scripts of the player controlled by client and enable it's rigidbody interpolation
/// for smooth movement</para>
/// </summary>
public class NetworkLazyClientPlayerSync : NetworkBehaviour
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