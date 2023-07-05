using UnityEngine;

/// <summary>
/// <para>Stores player state data</para>
/// </summary>
public class PlayerStateData : MonoBehaviour
{
    public enum PlayerState
    {
        NormalState,
        AbilityState,
        EasterEggState
    }

    [Header("State")]
    public PlayerState currentState = PlayerState.NormalState;

    [Header("Sub-states")]
    public bool isIdle;     //PlayerController.cs
    public bool isWalking;  //PlayerController.cs
    public bool isRunning;  //PlayerController.cs

    [Header("Logic Only Sub-states")]
    public bool isMoving;   //PlayerController.cs
    public bool isGrabbing; //PlayerGrabbing.cs
}