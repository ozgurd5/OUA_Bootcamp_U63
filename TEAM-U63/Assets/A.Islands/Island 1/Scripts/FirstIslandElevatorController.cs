using DG.Tweening;
using UnityEngine;

public class FirstIslandElevatorController : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private PressurePlate pressurePlate;
    [SerializeField] private float upFloorLocalY = 8;
    [SerializeField] private float moveTime = 2.3f;

    private AudioSource aus;

    private void Awake()
    {
        aus = GetComponent<AudioSource>();
        pressurePlate.OnPressurePlateInteraction += MoveElevator;
    }

    private void MoveElevator(bool isPressurePlatePressed)
    {
        //Don't change DoTween. It must be sync to the audio which last 2.3 seconds
        if (isPressurePlatePressed) transform.DOLocalMoveY(upFloorLocalY, moveTime);
        else transform.DOLocalMoveY(0f, moveTime);

        aus.Play();
    }
}
