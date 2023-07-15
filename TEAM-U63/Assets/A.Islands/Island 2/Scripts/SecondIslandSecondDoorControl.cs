using UnityEngine;

public class SecondIslandSecondDoorControl : MonoBehaviour
{ 
    private void Awake()
    {
        SecondIslandSecondLevelPuzzle.OnLevelCompleted += HandleForceField;
    }

    private void HandleForceField(bool isCompleted)
    {
        gameObject.SetActive(!isCompleted);
    }
}