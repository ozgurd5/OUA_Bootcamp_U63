using System;
using Cinemachine;
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

    private CinemachineFreeLook cam;
    private Rigidbody rb;

    private Vector3 lookingDirectionForward;
    private Vector3 lookingDirectionRight;

    private void Awake()
    {
        //TEST ONLY
        NetworkManager.Singleton.StartHost();
        //TEST ONLY

        npd = NetworkPlayerData.Singleton;
        nim = NetworkInputManager.Singleton;
        
        rb = GetComponent<Rigidbody>();
        cam = GetComponentInChildren<CinemachineFreeLook>();
        
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

    private void CalculateLookingDirections()
    {
        lookingDirectionForward = (transform.position - cam.transform.position).normalized;
        lookingDirectionForward.y = 0f;

        lookingDirectionRight = Vector3.Cross(Vector3.up, lookingDirectionForward);
        lookingDirectionRight.y = 0f;
        
        Debug.Log(lookingDirectionForward);
        Debug.Log(lookingDirectionRight);
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(input.moveInput.x, rb.velocity.y, input.moveInput.y) * moveSpeed;
        
        CalculateLookingDirections();
    }
}
