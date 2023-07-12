using UnityEngine;
using UnityEngine.UI;

public class PlayerQTEAbility : MonoBehaviour
{
    public static bool IsQTEActive;
    
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

    private PlayerInputManager pim;
    
    private float currentTimer;
    private int randomNumber;
    private int previousRandomNumber;
    
    [Header("Info - No Touch")]
    [SerializeField] private int currentKeyPress;
    private int hackDoneLimit = 4;

    private void Awake()
    {
        pim = GetComponent<PlayerInputManager>();
        
        GenerateRandomNumber();
        DisplayNumber();
    }

    private void Update()
    {
        if (!IsQTEActive) return;

        if (currentTimer > 0f) currentTimer -= Time.deltaTime;
        
        if (pim.isPrimaryAbilityKeyDown)
        {
            if (abilityCanvas.activeSelf) DeactivateCanvas();
            else ActivateCanvas();
        }
        
        if (currentTimer <= 0f) DeactivateCanvas();

        //If not pressing any key, return
        if (TurnInputToNumber() == 0) return;

        if (TurnInputToNumber() == randomNumber) Correct();
        else DeactivateCanvas();
    }

    private int TurnInputToNumber()
    {
        if (pim.qteUp) return 1;
        if (pim.qteDown) return 2;
        if (pim.qteRight) return 3;
        if (pim.qteLeft) return 4;
        
        return 0;
    }
    
    private void ActivateCanvas()
    {
        abilityCanvas.SetActive(true);
        currentTimer = keyTimerLimit;
    }
    
    private void DeactivateCanvas()
    {
        abilityCanvas.SetActive(false);
        if (IsHackQTE) currentKeyPress = 0;
    }
    
    private void Correct()
    {
        if (IsHackQTE)
        {
            currentKeyPress++;
            if (CheckHackCompletion()) DeactivateCanvas();
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