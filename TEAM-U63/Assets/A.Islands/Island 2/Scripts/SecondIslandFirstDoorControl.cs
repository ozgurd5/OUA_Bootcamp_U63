using UnityEngine;

public class SecondIslandFirstDoorControl : MonoBehaviour
{ 
    private void Awake()
    {
        SecondIslandFirstLevelPuzzle.OnLevelCompleted += HandleForceField;
    }

    private void HandleForceField(bool isCompleted)
    {
        gameObject.SetActive(!isCompleted);
    }
}
