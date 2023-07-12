using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

/// <summary>
/// <para>Responsible for both hacking and lullaby</para>
/// </summary>
public class PlayerQTEAbility : MonoBehaviour
{
    //TODO: make it false before build
    public static bool canQTE = true;
    public static event Action OnRobotStateChanged;
    public static event Action<Transform> OnRobotHacked;
    
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
    private CrosshairManager cm;
    
    private float currentTimer;
    private int randomNumber;
    private int previousRandomNumber;
    
    [Header("Info - No Touch")]
    [SerializeField] private int currentKeyPress;
    private int hackDoneLimit = 4;
    private int lullabyDoneLimit = 15;

    private RobotManager rm;

    private void Awake()
    {
        psd = GetComponent<PlayerStateData>();
        pim = GetComponent<PlayerInputManager>();
        cm = GetComponentInChildren<CrosshairManager>();
    }

    private void Update()
    {
        if (!canQTE) return;

        if (pim.isPrimaryAbilityKeyDown && cm.isLookingAtRobot)
        {
            rm = cm.crosshairHit.collider.GetComponent<RobotManager>();

            if (abilityCanvas.activeSelf) DeactivateAbility();
            
            if (!IsHackQTE) ActivateAbility();
            if (IsHackQTE && rm.isSleeping) ActivateAbility();
        }

        if (!abilityCanvas.activeSelf) return;
        
        if (currentTimer > 0f) currentTimer -= Time.deltaTime;
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
        psd.currentMainState = PlayerStateData.PlayerMainState.AbilityState;
        
        abilityCanvas.SetActive(true);
        currentTimer = keyTimerLimit;
        GenerateRandomNumber();
        DisplayNumber();
        
        //Sleeping process is necessary for robot to stand still while artist is singing
        //It can not be hacked during process. It can only be hacked while isSleeping is true
        if (!IsHackQTE)
        {
            rm.isSleepingProcess = true;
            OnRobotStateChanged?.Invoke();
        }
    }
    
    private void DeactivateAbility()
    {
        if (psd.currentMainState != PlayerStateData.PlayerMainState.RobotControllingState)
            psd.currentMainState = PlayerStateData.PlayerMainState.NormalState;

        abilityCanvas.SetActive(false);
        currentKeyPress = 0;
    }
    
    private void Correct()
    {
        currentKeyPress++;
        
        if (IsHackQTE && CheckHackCompletion()) DeactivateAbility();
        else if (!IsHackQTE && CheckLullabyCompletion()) DeactivateAbility();
        
        currentTimer = keyTimerLimit;
        GenerateRandomNumber();
        DisplayNumber();
    }

    #region CheckCompletion

    private bool CheckHackCompletion()
    {
        if (currentKeyPress == hackDoneLimit)
        {
            currentKeyPress = 0;
            
            rm.isHacked = true;

            psd.currentMainState = PlayerStateData.PlayerMainState.RobotControllingState;
            OnRobotStateChanged?.Invoke();
            OnRobotHacked?.Invoke(rm.transform);
            
            rm = null;
            
            return true;
        }
        
        return false;
    }

    private bool CheckLullabyCompletion()
    {
        if (currentKeyPress == lullabyDoneLimit)
        {
            currentKeyPress = 0;
            
            rm.isSleepingProcess = false;
            rm.isSleeping = true;
            
            OnRobotStateChanged?.Invoke();
            
            rm = null;
            
            return true;
        }
        
        return false;
    }

    #endregion

    #region RandomGenerationAndDisplay

    private void GenerateRandomNumber()
    {
        while (randomNumber == previousRandomNumber)
        {
            randomNumber = Random.Range(1, 5);
        }

        previousRandomNumber = randomNumber;
    }

    private Sprite TurnNumberToImage(int number)
    {
        if (number == 1) return upKeyImage;
        if (number == 2) return downKeyImage;
        if (number == 3) return rightKeyImage;
        if (number == 4) return leftKeyImage;

        return upKeyImage; //Won't be needed, compiler gets angry
    }
    
    private void DisplayNumber()
    {
        keyImage.sprite = TurnNumberToImage(randomNumber);
    }
    
    #endregion
}