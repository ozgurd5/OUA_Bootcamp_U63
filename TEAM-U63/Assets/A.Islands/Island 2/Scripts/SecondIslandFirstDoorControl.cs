using UnityEngine;

/// <summary>
/// <para>Opens and closes the door according to robot area completion</para>
/// </summary>
public class SecondIslandFirstDoorControl : MonoBehaviour
{ 
    private void Awake()
    {
        RobotAreaManager.OnRobotAreaCompleted += HandleForceField;
    }

    private void HandleForceField(bool isCompleted)
    {
        gameObject.SetActive(!isCompleted);
    }
}
