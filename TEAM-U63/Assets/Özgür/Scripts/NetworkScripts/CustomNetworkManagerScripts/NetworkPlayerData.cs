using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// <para>Responsible of deciding which user controls which player</para>
/// <para>Works both in host and client sides</para>
/// </summary>
public class NetworkPlayerData : NetworkBehaviour
{
    public static NetworkPlayerData Singleton;

    public event Action OnIsHostCoderChanged;

    /// <summary>
    /// <para>Is the coder controlled by host?</para>
    /// </summary>
    public bool isHostCoder { get; private set; }
    
    [Header("Debug Buttons")]
    [SerializeField] private bool invokeOnIsHostCoderChanged;
    [SerializeField] private bool printIsHostCoder;
    
    private void Awake()
    {
        Singleton = GetComponent<NetworkPlayerData>();

        //SceneManager.activeSceneChanged += (a, b) => { OnIsHostCoderChanged?.Invoke(); };
    }

    //DEBUG
    private void Update()
    {
        if (invokeOnIsHostCoderChanged)
        {
            OnIsHostCoderChanged?.Invoke();
            invokeOnIsHostCoderChanged = false;
        }

        else if (printIsHostCoder)
        {
            Debug.Log(isHostCoder);
            printIsHostCoder = false;
        }
        
        else if (Input.GetKeyDown(KeyCode.P))
        { 
            NetworkManager.Singleton.SceneManager.LoadScene("TEST", LoadSceneMode.Single);
        }
    }
    //DEBUG

    //We need a spawned network to subscribe OnClient.. actions
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        //Client must get the host isHostCoder value in the very start of the game - which is lobby screen
        NetworkManager.Singleton.OnClientConnectedCallback += obj => { if (IsHost) UpdateIsHostCoder(isHostCoder); };
    }

    /// <summary>
    /// <para>Updates which side controls which player</para>
    /// <para>Works and must work both in host side and client side</para>
    /// </summary>
    /// <param name="newIsHostCoder">Will host control coder after this action?</param>
    public void UpdateIsHostCoder(bool newIsHostCoder)
    {
        isHostCoder = newIsHostCoder;
        UpdateIsHostCoderClientRpc(newIsHostCoder);
        if (!IsHost) UpdateIsHostCoderServerRpc(newIsHostCoder);
        
        OnIsHostCoderChanged?.Invoke();
    }
    
    /// <summary>
    /// <para>Sends the date of which side controls which player to the host side</para>
    /// <para>Should not called by host side, though it's not important</para>
    /// </summary>
    /// <param name="newIsHostCoder">Will host control coder after this action?</param>
    [ServerRpc(RequireOwnership = false)]
    private void UpdateIsHostCoderServerRpc(bool newIsHostCoder)
    {
        isHostCoder = newIsHostCoder;
        OnIsHostCoderChanged?.Invoke();
    }
    
    /// <summary>
    /// <para>Sends the date of which side controls which player to the client</para>
    /// <para>Can't and shouldn't work in host side, though it's not important</para>
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