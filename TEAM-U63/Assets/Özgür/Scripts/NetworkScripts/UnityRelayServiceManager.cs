using System;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

/// <summary>
/// <para>Connects players through Unity Relay Service</para>
/// <para>Unity Relay Service is not a server or host, just connects host and client</para>
/// <para>All the data transfer between host and client goes through Unity Relay Service</para>
/// <para>So we don't need port forwarding or nat punch-through to connect host and client</para>
/// </summary>
public class UnityRelayServiceManager : MonoBehaviour
{
    public static string lobbyJoinCode;
    
    public static event Action OnLobbyJoinCodeCreated;
    public static event Action OnLobbyJoinCodeWrong;
    public static event Action OnLobbyJoinCodeCorrect;
    
    private async void Start()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    /// <summary>
    /// <para>Host creates server and sets the static joinCode variable</para>
    /// </summary>
    public static async void CreateRelay()
    {
        try
        {
            //Creates a "room" in Unity Relay Service side and sets it's data to allocation variable
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(1);
            
            //Creates a join code for the allocation, so the client can join the game with it
            lobbyJoinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            OnLobbyJoinCodeCreated?.Invoke();
            
            //Sets the network information about the allocation to relayServerData variable
            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
            
            //Sets the network information about the allocation to UnityTransport component so the host computer can connect Unity side
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            
            //Starts the host in host computer which is connected to Unity side
            NetworkManager.Singleton.StartHost();
        }
        
        catch(RelayServiceException exception)
        {
            Debug.Log(exception);
        }
    }
    
    /// <summary>
    /// <para>Client joins the server with the joinCode</para>
    /// </summary>
    /// <param name="joinCode">Join code from the host</param>
    public static async void JoinRelay(string joinCode)
    {
        try
        {
           //Sets the joinCode's allocation's data to the joinAllocation variable
           JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
           
           //Sets network information about the joinAllocation to relayServerData variable
           RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");
           
           //Sets the network information about the joinAllocation to UnityTransport component so the client computer can connect Unity side
           NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            
           //Starts the client in client computer which is connected to Unity side
           NetworkManager.Singleton.StartClient();
           
           OnLobbyJoinCodeCorrect?.Invoke();
        }
        
        catch (RelayServiceException exception)
        {
            Debug.Log(exception);
            
            OnLobbyJoinCodeWrong?.Invoke();
        }
    }
}
