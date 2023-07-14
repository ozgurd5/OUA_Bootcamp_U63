using UnityEngine;

/// <summary>
/// <para>Opens and closes the doors according to scale completion</para>
/// <para>Also enables the doors back in order to prevent player to go back</para>
/// </summary>
public class FirstIslandDoorControl : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private GameObject door;

    private void Awake()
    {
        ScaleController.OnScaleCompleted += HandleForceField;
    }

    private void HandleForceField(bool isCompleted)
    {
        door.SetActive(!isCompleted);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player")) door.SetActive(true);
    }
}