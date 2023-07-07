using System;
using Unity.Netcode;

/// <summary>
/// <para>Responsible of deciding which user controls which player</para>
/// <para>Works both in host and client side</para>
/// </summary>
public class NetworkPlayerData : NetworkBehaviour
{
    public static NetworkPlayerData Singleton;

    public event Action OnIsHostCoderChanged;

    public bool isHostCoder { get; private set; }
    
    private void Awake()
    {
        Singleton = GetComponent<NetworkPlayerData>();
    }

    /// <summary>
    /// <para>Updates current state of the selected players through the network</para>
    /// <para>Works and must work both in host side and client side</para>
    /// </summary>
    /// <param name="newIsHostCoder">Will host control coder after this action?</param>
    public void UpdateIsHostCoder(bool newIsHostCoder)
    {
        isHostCoder = newIsHostCoder;
        OnIsHostCoderChanged?.Invoke();
        
        if (!IsHost) UpdateIsHostCoderServerRpc(newIsHostCoder);
        UpdateIsHostCoderClientRpc(newIsHostCoder);
    }
    
    /// <summary>
    /// <para>Sends current state of the selected players to host side</para>
    /// <para>Should not work in host side, though it's not important</para>
    /// </summary>
    /// <param name="newIsHostCoder">Will host control coder after this action?</param>
    [ServerRpc(RequireOwnership = false)]
    private void UpdateIsHostCoderServerRpc(bool newIsHostCoder)
    {
        isHostCoder = newIsHostCoder;
        OnIsHostCoderChanged?.Invoke();
    }
    
    /// <summary>
    /// <para>Sends current state of the selected players to client side</para>
    /// <para>Can't work in host side, though it's not important</para>
    /// </summary>
    /// <param name="newIsHostCoder">Will host control coder after this action?</param>
    [ClientRpc]
    private void UpdateIsHostCoderClientRpc(bool newIsHostCoder)
    {
        if (IsHost) return;
        isHostCoder = newIsHostCoder;
        OnIsHostCoderChanged?.Invoke();
    }
}