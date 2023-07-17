using UnityEngine;

public class SecondIslandThirdDoorControl : MonoBehaviour
{
    private void Awake()
    {
        LaserTarget.OnLaserTargetHit += HandleForceField;
    }
    
    private void HandleForceField(bool isCompleted)
    {
        gameObject.SetActive(!isCompleted);
    }

    private void OnDestroy()
    {
        LaserTarget.OnLaserTargetHit -= HandleForceField;
    }
}
