using UnityEngine;

public class RobotSoundManager : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private AudioSource aus;
    [SerializeField] private AudioClip robotSleptSound;
    [SerializeField] private AudioClip robotHackedSound;
    
    private RobotManager rm;
    
    public void Awake()
    {
         rm = GetComponent<RobotManager>();
         rm.OnRobotStateChanged += PlayRobotStateSounds;
    }

    private void PlayRobotStateSounds()
    {
        if (rm.currentState == RobotManager.RobotState.Sleeping) aus.PlayOneShot(robotSleptSound);
        else if (rm.currentState == RobotManager.RobotState.Hacked) aus.PlayOneShot(robotHackedSound);
    }
}
