using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    [Header("IMPORTANT - SELECT")]
    [SerializeField] private bool isCoder;
    
    private NetworkInputManager nim;
    private NetworkInputManager.InputData input;
    
    private void Awake()
    {
        nim = NetworkInputManager.Singleton;

        if (isCoder)
            input = nim.coderInput;
        else
            input = nim.artistInput;
    }
    
    
}
