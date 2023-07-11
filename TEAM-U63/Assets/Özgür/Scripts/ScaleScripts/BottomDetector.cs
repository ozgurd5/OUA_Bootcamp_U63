using System;
using UnityEngine;

/// <summary>
/// <para>Detects if there are any object bellow the assigned scale</para>
/// </summary>
public class BottomDetector : MonoBehaviour
{
    public bool isTouching;
    public event Action OnBottomExit; 

    private void OnTriggerEnter(Collider col)
    {
        isTouching = true;
    }

    private void OnTriggerExit(Collider col)
    {
        isTouching = false;
        OnBottomExit?.Invoke(); //We must update scale position when the bottom is clear
    }
}
