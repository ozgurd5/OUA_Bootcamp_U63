using System;
using Unity.Netcode;
using UnityEngine;

public class NetworkData : NetworkBehaviour
{
    public static bool isClientInGame;
    public static NetworkVariable<bool> isHostCoder;
    
    private void Awake()
    {
        isHostCoder = new NetworkVariable<bool>(true);
    }
}
