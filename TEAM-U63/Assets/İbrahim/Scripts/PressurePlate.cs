using System;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private string activationTag = "Player";

    [Header("Info - No touch")]
    public bool isPressed;
    public event Action<bool> OnPressurePlateInteraction; 

    private AudioSource aus;

    private void Awake()
    {
        aus = GetComponent<AudioSource>();
    }
    
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag(activationTag) && !isPressed)
        {
            isPressed = true;
            aus.Play();
            OnPressurePlateInteraction?.Invoke(isPressed);
        }
    }
    
    private void OnTriggerExit(Collider col)
    {
        if (!col.CompareTag(activationTag)) return;
        
        isPressed = false;
        aus.Play();
        OnPressurePlateInteraction?.Invoke(isPressed);
    }
}