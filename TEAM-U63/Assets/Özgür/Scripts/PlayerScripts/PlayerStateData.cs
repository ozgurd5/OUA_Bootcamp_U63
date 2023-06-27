using UnityEngine;

public class PlayerStateData : MonoBehaviour
{
    public enum PlayerState
    {
        NormalState,
        AbilityState
    }

    [Header("State")]
    public PlayerState currentState = PlayerState.NormalState;

    [Header("Sub-states")]
    public bool isMoving;   //PlayerController.cs
    public bool isWalking;  //PlayerController.cs
    public bool isRunning;  //PlayerController.cs
    public bool isJumping;
    public bool isGrabbing;

    [Header("Logic Only Sub-states")]
    public bool isGrounded; //PlayerGroundChecker.cs
}
