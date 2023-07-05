using UnityEngine;

public class FirstIslandDoorForceFieldDisabler : MonoBehaviour
{
    private void Awake()
    {
        ScaleController.OnScaleCompleted += HandleForceField;
    }

    private void HandleForceField(bool isCompleted)
    {
        gameObject.SetActive(!isCompleted);
    }
}