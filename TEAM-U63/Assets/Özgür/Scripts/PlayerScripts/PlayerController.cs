using System;
using Unity.Netcode;
using UnityEngine;

//TODO: remove test only line in awake before build

public class PlayerController : NetworkBehaviour
{
    [Header("IMPORTANT - SELECT")]
    [SerializeField] private bool isCoder;

    [Header("Assign")]
    [SerializeField] private float moveSpeed;

    private NetworkPlayerData npd;
    private NetworkInputManager nim;
    private NetworkInputManager.InputData input;

    private Rigidbody rb;
    
    private void Awake()
    {
        //TEST ONLY
        NetworkManager.Singleton.StartHost();
        //TEST ONLY

        npd = NetworkPlayerData.Singleton;
        nim = NetworkInputManager.Singleton;
        
        rb = GetComponent<Rigidbody>();
        
        DecideForInputSource();
        npd.OnIsHostCoderChanged += obj => DecideForInputSource();
    }
    
    /// <summary>
    /// <para>Gets coder or artist input as the input source</para>
    /// </summary>
    private void DecideForInputSource()
    {
        if (isCoder)
            input = nim.coderInput;
        else
            input = nim.artistInput;
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(input.moveInput.x, rb.velocity.y, input.moveInput.y) * moveSpeed;
    }
}
