using DG.Tweening;
using UnityEngine;

public class FirstIslandElevatorController : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private PressurePlate pressurePlate;
    [SerializeField] private float upFloorLocalY = 8;
    [SerializeField] private float moveTime = 1.5f;

    private AudioSource aus;

    private void Awake()
    {
        aus = GetComponent<AudioSource>();
        pressurePlate.OnPressurePlateInteraction += MoveElevator;
    }

    private void MoveElevator(bool isPressurePlatePressed)
    {
        if (isPressurePlatePressed) transform.DOLocalMoveY(upFloorLocalY, moveTime);
        else transform.DOLocalMoveY(0f, moveTime);
        
        aus.Play();
    }
}
