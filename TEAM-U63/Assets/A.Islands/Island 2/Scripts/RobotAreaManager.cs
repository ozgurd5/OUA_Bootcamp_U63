using System;
using UnityEngine;

public class RobotAreaManager : MonoBehaviour
{
    public static event Action<bool> OnRobotAreaCompleted;
    private static int robotNumber;

    [Header("Assign")]
    [SerializeField] private AudioClip completedClip;
    [SerializeField] private AudioClip allCompletedClip;

    private AudioSource aus;
    private bool isCompleted;

    private void Awake()
    {
        aus = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (!col.CompareTag("robot")) return;

        isCompleted = true;
        robotNumber++;
        CheckAllCompletion();
    }

    private void OnTriggerExit(Collider col)
    {
        if (!col.CompareTag("robot")) return;

        isCompleted = false;
        robotNumber--;
        CheckAllCompletion();
    }

    private void CheckAllCompletion()
    {
        if (robotNumber == 2)
        {
            aus.PlayOneShot(allCompletedClip);
            OnRobotAreaCompleted?.Invoke(true);
        }
        
        else
        {
            if (isCompleted) aus.PlayOneShot(completedClip);
            OnRobotAreaCompleted?.Invoke(false);
        }
    }
}
