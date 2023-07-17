using Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

//TODO: random numbers and inputs not 1-4 but 0-3
/// <summary>
/// <para>Responsible for both hacking and lullaby</para>
/// </summary>
public class PlayerQTEAbility : MonoBehaviour
{
    [Header("ASSIGN - BROKEN")]
    [SerializeField] CrosshairManager cm;
    
    [Header("Assign - Images")]
    [SerializeField] private Sprite upKeyImage;
    [SerializeField] private Sprite downKeyImage;
    [SerializeField] private Sprite rightKeyImage;
    [SerializeField] private Sprite leftKeyImage;
    
    [Header("Assign")]
    [SerializeField] private float keyTimerLimit = 2f;
    [SerializeField] private Image keyImage;
    [SerializeField] private GameObject abilityCanvas;
    
    [Header("Assign - Sounds")]
    [SerializeField] private AudioSource aus;
    [SerializeField] private AudioClip artistFailClip;
    [SerializeField] private AudioClip coderCorrectClip;
    [SerializeField] private AudioClip coderFailClip;

    private bool IsHackQTE;

    private PlayerData pd;
    private PlayerStateData psd;
    private PlayerInputManager pim;

    private CinemachineFreeLook cam;
    private Rigidbody rb;
    
    private float currentTimer;
    private int randomNumber;
    private int previousRandomNumber;

    [Header("Info - No Touch")]
    [SerializeField] private int currentKeyPress;
    private int hackDoneLimit = 6;
    private int lullabyDoneLimit = 15;

    private RobotManager rm;

    private void Awake()
    {
        IsHackQTE = name == "CoderPlayer";

        pd = GetComponent<PlayerData>();
        psd = GetComponent<PlayerStateData>();
        pim = GetComponent<PlayerInputManager>();
        //cm = GetComponentInChildren<CrosshairManager>();
        cam = GetComponentInChildren<CinemachineFreeLook>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!pd.isLocal) return;
        
        if (pim.isPrimaryAbilityKeyDown && cm.isLookingAtRobot)
        {
            rm = cm.crosshairHit.collider.GetComponent<RobotManager>();

            if (psd.currentMainState == PlayerStateData.PlayerMainState.AbilityState) DeactivateAbility(false);
            
            else if (!IsHackQTE && rm.currentState == RobotManager.RobotState.Routing) ActivateAbility();
            else if (IsHackQTE && rm.currentState == RobotManager.RobotState.Sleeping) ActivateAbility();
        }
        
        if (psd.currentMainState != PlayerStateData.PlayerMainState.AbilityState) return;
        
        if (currentTimer > 0f) currentTimer -= Time.deltaTime;
        if (currentTimer <= 0f) DeactivateAbility(false);

        //If not pressing any key, return
        if (TurnInputToNumber() == 0) return;

        if (TurnInputToNumber() == randomNumber) Correct();
        else DeactivateAbility(false);
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
        rb.velocity = Vector3.zero;
        
        abilityCanvas.SetActive(true);
        currentTimer = keyTimerLimit;
        GenerateRandomNumber();
        DisplayNumber();
        
        //Sleeping process is necessary for robot to stand still while artist is singing
        //It can not be hacked during process. It can only be hacked while isSleeping is true
        if (!IsHackQTE) rm.UpdateRobotState((int)RobotManager.RobotState.SleepingProcess);
    }
    
    private void DeactivateAbility(bool isSucceeded)
    {
        if (isSucceeded)
        {
            if (IsHackQTE)
            {
                aus.PlayOneShot(coderCorrectClip);
                
                //Responsibility chart of the states/rigidbody/camera
                //1.a Robot - Hacked Enter - PlayerQTEAbility.cs and RobotManager.cs
                //1.b Player - RobotControllingState Enter - PlayerQTEAbility.cs
                //2.a Robot - Hacked Exit to Sleeping - RobotManager.cs
                //2.b Player - RobotControllingState Exit to NormalState - PlayerController.cs
            
                //1.a
                rm.UpdateRobotState((int)RobotManager.RobotState.Hacked);
            
                //1.b
                psd.currentMainState = PlayerStateData.PlayerMainState.RobotControllingState;
                rb.constraints = RigidbodyConstraints.FreezeAll;
                cam.enabled = false;
            }
            
            //Only artist returns to normal state after success
            else
            {
                psd.currentMainState = PlayerStateData.PlayerMainState.NormalState;
                rm.UpdateRobotState((int)RobotManager.RobotState.Sleeping);
            }
        }
        
        else
        {
            //Both player returns to normal state after failure
            psd.currentMainState = PlayerStateData.PlayerMainState.NormalState;

            if (IsHackQTE) aus.PlayOneShot(coderFailClip);
            
            else
            {
                rm.UpdateRobotState((int)RobotManager.RobotState.Routing);
                aus.PlayOneShot(artistFailClip);
            }
        }

        abilityCanvas.SetActive(false);
        currentKeyPress = 0;
    }
    
    private void Correct()
    {
        currentKeyPress++;
        
        if (IsHackQTE && CheckCompletion(hackDoneLimit)) DeactivateAbility(true);
        else if (IsHackQTE) aus.PlayOneShot(coderCorrectClip);
        else if (!IsHackQTE && CheckCompletion(lullabyDoneLimit)) DeactivateAbility(true);
        
        currentTimer = keyTimerLimit;
        GenerateRandomNumber();
        DisplayNumber();
    }
    
    private bool CheckCompletion(int doneLimit)
    {
        if (currentKeyPress != doneLimit) return false;
        
        currentKeyPress = 0;
        return true;

    }

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