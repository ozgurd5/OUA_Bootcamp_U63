using UnityEngine;

public class RobotManager : MonoBehaviour
{
    [Header("Robot States")]
    public bool isSleeping;         //PlayerQTEAbility.cs
    public bool isHacked;           //PlayerQTEAbility.cs
    
    //Sleeping process is necessary for robot to stand still while artist is singing
    //It can not be hacked during process. It can only be hacked while isSleeping is true
    public bool isSleepingProcess;  //PlayerQTEAbility.cs

    private RobotController rb;

    private void Awake()
    {
        rb = GetComponent<RobotController>();

        PlayerQTEAbility.OnRobotStateChanged += UpdateRobotControlStatus;
    }

    private void UpdateRobotControlStatus()
    {
        rb.enabled = isHacked;
    }
}
