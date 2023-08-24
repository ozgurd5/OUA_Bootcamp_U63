using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// <para>Gather and holds data from the unity input system</para>
/// <para>Works for only local player</para>
/// </summary>
public class PlayerInputManager : MonoBehaviour
{
    private PlayerInputActions pia;
    private PlayerData pd;
    
    public bool qteUp;
    public bool qteDown;
    public bool qteLeft;
    public bool qteRight;

    public Vector2 lookInput;
    public Vector2 moveInput;
    public bool isRunKey;
    public bool isGrabKeyDown;
    public bool isPrimaryAbilityKeyDown;
    public bool isSecondaryAbilityKeyDown;
    public bool isEasterEggKeyDown;
    public bool isEasterEggKeyUp;
    public bool isMapKeyDown;
    public bool isPauseKeyDown;
    public bool isSkipDialogueKeyDown;

    private void Awake()
    {
        pia = new PlayerInputActions();
        pia.Player.Enable();
        pd = GetComponent<PlayerData>();
    }

    private void Update()
    {
        if (!pd.isLocal) return;

        //QTE keys can not be Vector2, we need buttons
        qteUp = pia.Player.QTEUp.WasPressedThisFrame();
        qteDown = pia.Player.QTEDown.WasPressedThisFrame();
        qteLeft = pia.Player.QTELeft.WasPressedThisFrame();
        qteRight = pia.Player.QTERight.WasPressedThisFrame();

        lookInput = pia.Player.Look.ReadValue<Vector2>() * 0.2f;
        moveInput = pia.Player.Movement.ReadValue<Vector2>() * 0.2f;
        isRunKey = pia.Player.Run.IsPressed();
        isGrabKeyDown = pia.Player.Grab.WasPressedThisFrame();
        isPrimaryAbilityKeyDown = pia.Player.PrimaryAbility.WasPressedThisFrame();
        isSecondaryAbilityKeyDown = pia.Player.SecondaryAbility.WasPressedThisFrame();
        isEasterEggKeyDown = pia.Player.EasterEgg.WasPressedThisFrame();
        isMapKeyDown = pia.Player.MapKey.WasPressedThisFrame();
        isEasterEggKeyUp = pia.Player.EasterEgg.WasReleasedThisFrame();
        isPauseKeyDown = pia.Player.PauseMenu.WasPressedThisFrame();
        isSkipDialogueKeyDown = pia.Player.SkipDialogue.WasPressedThisFrame();
    }
}
