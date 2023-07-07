using System;
using UnityEngine;

/// <summary>
/// <para>Responsible of vision ability of the coder</para>
/// </summary>
public class PlayerCoderVisionAbility : MonoBehaviour
{
    public static bool isCoderVisionActive;
    public static event Action OnCoderVisionEnable;
    
    [Header("Assign")]
    [SerializeField] private GameObject coderVisionCanvas;

    private PlayerController pc;

    private void Awake()
    {
        pc = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (!pc.input.isSecondaryAbilityKeyDown) return;
        
        isCoderVisionActive = !isCoderVisionActive;
        coderVisionCanvas.SetActive(isCoderVisionActive);
        
        OnCoderVisionEnable?.Invoke();
    }
}