using UnityEngine;

public class RobotAnimationManager : MonoBehaviour
{
    [SerializeField] public GameObject blinkEyesObject;
    [SerializeField] public GameObject hypnotizedEyesObject;
    [SerializeField] public GameObject hypnotizedEarsObject;
    [SerializeField] public GameObject normalEarsObject;
    
    private RobotManager rm;

    public void Awake()
    {
         rm = GetComponent<RobotManager>();
         rm.OnRobotStateChanged += UpdateRobotAnimations;
    }

    private void UpdateRobotAnimations()
    {
        if (rm.isSleepingProcess || rm.isSleeping || rm.isHacked)
        {
            blinkEyesObject.SetActive(false);
            normalEarsObject.SetActive(false);
            hypnotizedEarsObject.SetActive(true);
            hypnotizedEyesObject.SetActive(true);
        }
        
        else
        {
            blinkEyesObject.SetActive(true);
            normalEarsObject.SetActive(true);
            hypnotizedEarsObject.SetActive(false);
            hypnotizedEyesObject.SetActive(false);
        }
    }
}
