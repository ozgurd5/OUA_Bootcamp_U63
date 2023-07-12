using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// <para>Responsible for both hacking and lullaby</para>
/// </summary>
public class PlayerQTEAbility : MonoBehaviour
{
    //TODO: make it false before build
    public static bool canQTE = true;
    public static bool isCurrentlySinging;
    
    [Header("MAKE IT TRUE IF THIS SCRIPT IS FOR HACKING, FALSE FOR LULLABY")]
    [SerializeField] private bool IsHackQTE;

    [Header("Assign - Images")]
    [SerializeField] private Sprite upKeyImage;
    [SerializeField] private Sprite downKeyImage;
    [SerializeField] private Sprite rightKeyImage;
    [SerializeField] private Sprite leftKeyImage;
    
    [Header("Assign")]
    [SerializeField] private float keyTimerLimit = 2f;
    [SerializeField] private Image keyImage;
    [SerializeField] private GameObject abilityCanvas;

    private PlayerStateData psd;
    private PlayerInputManager pim;
    
    private float currentTimer;
    private int randomNumber;
    private int previousRandomNumber;
    
    [Header("Info - No Touch")]
    [SerializeField] private int currentKeyPress;
    private int hackDoneLimit = 4;

    private void Awake()
    {
        psd = GetComponent<PlayerStateData>();
        pim = GetComponent<PlayerInputManager>();
        
        GenerateRandomNumber();
        DisplayNumber();
    }

    private void Update()
    {
        if (!canQTE) return;

        if (currentTimer > 0f) currentTimer -= Time.deltaTime;
        
        if (pim.isPrimaryAbilityKeyDown)
        {
            if (abilityCanvas.activeSelf) DeactivateAbility();
            else ActivateAbility();
        }
        
        if (currentTimer <= 0f) DeactivateAbility();

        //If not pressing any key, return
        if (TurnInputToNumber() == 0) return;

        if (TurnInputToNumber() == randomNumber) Correct();
        else DeactivateAbility();
    }

    private int TurnInputToNumber()
    {
        if (pim.qteUp) return 1;
        if (pim.qteDown) return 2;
        if (pim.qteRight) return 3;
        if (pim.qteLeft) return 4;
        
        return 0;
    }
    
    private void ActivateAbility()
    {
        if (!IsHackQTE) isCurrentlySinging = true;
        
        psd.currentMainState = PlayerStateData.PlayerMainState.AbilityState;
        
        abilityCanvas.SetActive(true);
        currentTimer = keyTimerLimit;
    }
    
    private void DeactivateAbility()
    {
        if (!IsHackQTE) isCurrentlySinging = false;
        
        psd.currentMainState = PlayerStateData.PlayerMainState.NormalState;
        
        abilityCanvas.SetActive(false);
        if (IsHackQTE) currentKeyPress = 0;
    }
    
    private void Correct()
    {
        if (IsHackQTE)
        {
            currentKeyPress++;
            if (CheckHackCompletion()) DeactivateAbility();
        }
        
        currentTimer = keyTimerLimit;
        GenerateRandomNumber();
        DisplayNumber();
    }
    
    private bool CheckHackCompletion()
    {
        if (currentKeyPress == hackDoneLimit)
        {
            currentKeyPress = 0;
            return true;
        }
        
        return false;
    }
    
    private void GenerateRandomNumber()
    {
        while (randomNumber == previousRandomNumber)
        {
            randomNumber = Random.Range(1, 5);
        }

        previousRandomNumber = randomNumber;
    }

    private void DisplayNumber()
    {
        keyImage.sprite = TurnNumberToImage(randomNumber);
    }

    private Sprite TurnNumberToImage(int number)
    {
        if (number == 1) return upKeyImage;
        if (number == 2) return downKeyImage;
        if (number == 3) return rightKeyImage;
        if (number == 4) return leftKeyImage;

        return upKeyImage; //Won't be needed, compiler gets angry
    }
}