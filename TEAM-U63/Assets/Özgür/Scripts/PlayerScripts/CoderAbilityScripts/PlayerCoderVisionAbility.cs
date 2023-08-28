using System;
using Unity.Netcode;
using UnityEngine;

/// <summary>
/// <para>Responsible of vision ability of the coder</para>
/// <para>Works only in local player</para>
/// </summary>
public class PlayerCoderVisionAbility : NetworkBehaviour
{
    [Header("Assign - Coder Vision Audio Source")]
    [SerializeField] private AudioSource aus;
    
    public static bool isCoderVisionActive;
    public static event Action OnCoderVisionEnable;
    
    [Header("Assign")]
    [SerializeField] private Canvas coderVisionCanvas;

    private PlayerData pd;
    private PlayerStateData psd;
    private PlayerInputManager pim;

    private void Awake()
    {
        pd = GetComponent<PlayerData>();
        psd = GetComponent<PlayerStateData>();
        pim = GetComponent<PlayerInputManager>();
    }

    void Update()
    {
        if (psd.currentMainState == PlayerStateData.PlayerMainState.AbilityState || !pd.isLocal)
        {
            isCoderVisionActive = false;
            coderVisionCanvas.gameObject.SetActive(false);

            return;
        }
        
        if (!pim.isSecondaryAbilityKeyDown) return;

        isCoderVisionActive = !isCoderVisionActive;
        coderVisionCanvas.gameObject.SetActive(isCoderVisionActive);
        aus.Play();

        OnCoderVisionEnable?.Invoke();
    }
}