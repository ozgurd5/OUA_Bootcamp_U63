using Unity.Netcode;
using UnityEngine;

/// <summary>
/// <para>Handles synchronization of position and rotation across the network with interpolation</para>
/// <para>Works only for local object</para>
/// <para>Local objects transmit position and rotation data, remote objects receive position and rotation data</para>
/// </summary>
public class NetworkSyncPositionAndRotation : NetworkBehaviour
{
    private static float interpolationValue = 0.5f;
    private static float defaultRotationSpeed = 0.5f;
    
    private PlayerData pd;
    private CubeManager cm;
    private RobotManager rm;
    
    private PlayerController pc;
    
    private float rotatingSpeed;
    [SerializeField] private bool isObjectLocal;

    private void Awake()
    {
        if (CompareTag("Player"))
        {
            pd = GetComponent<PlayerData>();
            pd.OnLocalStatusChanged += () => { isObjectLocal = pd.isLocal; };
            isObjectLocal = pd.isLocal;

            pc = GetComponent<PlayerController>();
        }
        
        else if (CompareTag("RedPuzzle") || CompareTag("GreenPuzzle") || CompareTag("BluePuzzle"))
        {
            cm = GetComponent<CubeManager>();
            cm.OnLocalStatusChanged += () => { isObjectLocal = cm.isLocal; };
            isObjectLocal = cm.isLocal;
        }
        
        else if (CompareTag("Robot"))
        {
            rm = GetComponent<RobotManager>();
            rm.OnLocalStatusChanged += () => { isObjectLocal = rm.isLocal; };
            isObjectLocal = rm.isLocal;
        }

        //Other objects should be host controlled by default
        else isObjectLocal = IsHost;
    }

    private void Update()
    {
        if (!isObjectLocal) return;
        
        //transform.position in this line is the position in the host side because client can't call ClientRpc
        SyncClientPositionWithInterpolationClientRpc(transform.position);
            
        //transform.position in this line must be states in the client side and it is
        if (!IsHost) SyncHostPositionWithInterpolationServerRpc(transform.position);

        //Player's rotating speed is changes according to walking or running, other objects are constant
        if (CompareTag("Player"))
            rotatingSpeed = pc.rotatingSpeed;
        else
            rotatingSpeed = defaultRotationSpeed;
        
        //transform.forward in this line is the position in the host side because client can't call ClientRpc
        SyncClientRotationWithInterpolationClientRpc(transform.forward, rotatingSpeed);
            
        //transform.forward in this line is the position in the client side and it must be
        if (!IsHost) SyncHostRotationWithInterpolationServerRpc(transform.forward, rotatingSpeed);
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
        if (!IsHost) transform.position = Vector3.Lerp(transform.position , hostPosition, interpolationValue);
    }

    /// <summary>
    /// <para>Sends position in the client to host and interpolates it in host side</para>
    /// <para>Must not called by the host, be careful. Since host is also a client, it can call this method. If so,
    /// that would override client side position and cause object to not update it's position</para>
    /// </summary>
    /// <param name="clientPosition">Position in the client side</param>
    [ServerRpc(RequireOwnership = false)]
    private void SyncHostPositionWithInterpolationServerRpc(Vector3 clientPosition)
    {
        transform.position = Vector3.Lerp(transform.position , clientPosition, interpolationValue);
    }

    /// <summary>
    /// <para>Sends rotation in the host to client and interpolates it in client side</para>
    /// <para>Can't and must not work in host side</para>
    /// </summary>
    /// <param name="hostForward">Position in the host side</param>
    /// <param name="hostRotationSpeed">Rotation speed in the host side which changes according to running or walking states</param>
    [ClientRpc]
    private void SyncClientRotationWithInterpolationClientRpc(Vector3 hostForward, float hostRotationSpeed)
    {
        //Since host is also a client, it will also try to run this method. It must not //TODO: what happens if it does?
        if (!IsHost) transform.forward = Vector3.Lerp(transform.forward , hostForward, hostRotationSpeed);
    }
    
    /// <summary>
    /// <para>Sends position in the client to host and interpolates it in host side</para>
    /// <para>Must not called by the host, be careful. Since host is also a client, it can call this method. If so,
    /// that would override client side rotation and cause object to not update it's rotation</para>
    /// </summary>
    /// <param name="clientForward">Position in the client side</param>
    /// <param name="clientRotationSpeed">Rotation speed in the client side which changes according to running or walking states</param>
    [ServerRpc(RequireOwnership = false)]
    private void SyncHostRotationWithInterpolationServerRpc(Vector3 clientForward, float clientRotationSpeed)
    {
        transform.forward = Vector3.Slerp(transform.forward, clientForward, clientRotationSpeed);
    }
}