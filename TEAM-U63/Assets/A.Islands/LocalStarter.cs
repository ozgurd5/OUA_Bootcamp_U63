using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class LocalStarter : MonoBehaviour
{
    //[Header("Assign")]
    //[SerializeField] private Button hostButton;
    //[SerializeField] private Button joinButton;
    //
    //private void Awake()
    //{
    //    throw new NotImplementedException();
    //}

    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }
}
