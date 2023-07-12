using System;
using Unity.Netcode;
using UnityEngine;

/// <summary>
/// <para>Responsible of vision ability of the coder</para>
/// <para>Works only in local player</para>
/// </summary>
public class PlayerCoderVisionAbility : NetworkBehaviour
{
    public static bool isCoderVisionActive;
    public static event Action OnCoderVisionEnable;
    
    [Header("Assign")]
    [SerializeField] private Canvas coderVisionCanvas;

    private PlayerInputManager pim;

    private void Awake()
    {
        pim = GetComponent<PlayerInputManager>();
    }

    void Update()
    {
        if (!pim.isSecondaryAbilityKeyDown) return;

        isCoderVisionActive = !isCoderVisionActive;
        coderVisionCanvas.gameObject.SetActive(isCoderVisionActive);

        OnCoderVisionEnable?.Invoke();
    }
}