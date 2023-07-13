using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>Stupid Netcode doesn't allow clients to change parenting but it's necessary for smooth movement in elevator,
/// scale etc. Stupid Netcode also doesn't know how to serialize a Transform, GameObject or even NetworkObject. Therefore
/// we can't even call a ServerRpc in client side and pass the object itself, it's transform or network object components
/// as a parameter to change the parenting in host side. We can only pass primitive types or things can be represent as
/// primitive types after serialization. So we need a solution for client side parenting.</para>
/// <para>What this script do is it's holding the transform component of the objects that will become a parent during
/// the gameplay and use their indexes in the NetworkParentList as their id, so client can use that ids as the parameter
/// in the parent changer ServerRpc methods and reach the transform component of the object that will become a parent</para>
/// <para>Good thing is host side parenting are synced automatically through network object component so we don't need
/// to call client rpc for that</para>
/// </summary>
public class NetworkParentingManager : MonoBehaviour
{
    public static NetworkParentingManager Singleton;

    //Level 1 (order is not important in the list but assign their indexes to the responsible scripts in the objects)
    //GrabPoint - 0 - PlayerGrabbing.cs
    //Left-Mid-Right Scales - 1-2-3 - ScaleController.cs
    //Elevator Parenter - 4 - Elevator Parenter
    
    [Header("Assign Parents - Their indexes are their IDs - Look for the notes in the script")]
    public List<Transform> NetworkParentList;

    private void Awake()
    {
        Singleton = GetComponent<NetworkParentingManager>();
    }
    
    /// <summary>
    /// <para>Returns the corresponding transform in the NetworkParentList</para>
    /// </summary>
    /// <param name="networkParentListID">Index of the transform in the list</param>
    /// <returns>The corresponding transform in the NetworkParentList</returns>
    public Transform FindTransformUsingID(int networkParentListID)
    {
        return NetworkParentList[networkParentListID];
    }
}
