using System;
using Unity.Netcode;
using UnityEngine;

/// <summary>
/// <para>Responsible of vision ability of the coder</para>
/// </summary>
public class PlayerCoderVisionAbility : NetworkBehaviour
{
    //
    private NetworkPlayerData npd;
    
    public static bool isCoderVisionActive;
    public static event Action OnCoderVisionEnable;
    
    [Header("Assign")]
    [SerializeField] private Canvas coderVisionCanvas;

    private PlayerController pc;

    private void Awake()
    {
        pc = GetComponent<PlayerController>();
        
        //
        npd = NetworkPlayerData.Singleton;
    }

    void Update()
    {
        //
        if ((npd.isHostCoder && IsHost) || (!npd.isHostCoder && !IsHost))
        {
            if (!pc.input.isSecondaryAbilityKeyDown) return;

            isCoderVisionActive = !isCoderVisionActive;
            coderVisionCanvas.gameObject.SetActive(isCoderVisionActive);

            OnCoderVisionEnable?.Invoke();
        }
    }
}