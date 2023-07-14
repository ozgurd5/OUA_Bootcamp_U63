using System;
using UnityEngine;

public class SecondIslandSecondLevelPuzzle : MonoBehaviour
{
    public static event Action<bool> OnLevelCompleted;
    private static bool isAllCompleted;
    
    //TODO: FIND A BETTER SOLUTION
    public static bool[] completionStatus = new bool[4];
    [Header("ASSIGN ID")]
    [SerializeField] private int ID;
    
    [Header("SELECT THE COLLIDER TYPE")]
    [SerializeField] private bool IsRobot;

    [Header("Assign - If Cube")]
    [SerializeField] private int neededRed;
    [SerializeField] private int neededGreen;
    [SerializeField] private int neededBlue;

    [Header("Info - No touch")]
    public bool isCompleted;
    
    private int redNumber;
    private int greenNumber;
    private int blueNumber;

    private void OnTriggerEnter(Collider col)
    {
        if (IsRobot && col.CompareTag("robot"))
        {
            Debug.Log("enter - name: " + name);
            isCompleted = true;
            completionStatus[ID] = true;
            CheckAllCompletion();
        }
        
        else
        {
            Debug.Log("enter - name: " + name);
            if (col.CompareTag("RedPuzzle")) redNumber++;
            if (col.CompareTag("GreenPuzzle")) greenNumber++;
            if (col.CompareTag("BluePuzzle")) blueNumber++;

            CheckCubeCompletion();
            CheckAllCompletion();
        }
    }
    
    private void OnTriggerExit(Collider col)
    {
        if (IsRobot && col.CompareTag("robot"))
        {
            isCompleted = false;
            completionStatus[ID] = false;
            CheckAllCompletion();
        }
        
        else
        {
            if (col.CompareTag("RedPuzzle")) redNumber--;
            if (col.CompareTag("GreenPuzzle")) greenNumber--;
            if (col.CompareTag("BluePuzzle")) blueNumber--;

            CheckCubeCompletion();
            CheckAllCompletion();
        }
    }

    private void CheckCubeCompletion()
    {
        isCompleted = redNumber == neededRed && greenNumber == neededGreen && blueNumber == neededBlue;
        completionStatus[ID] = isCompleted;
    }

    //TODO: FIND BETTER SOLUTION
    private void CheckAllCompletion()
    {
        isAllCompleted = completionStatus[0] && completionStatus[1] && completionStatus[2] && completionStatus[3];
        OnLevelCompleted?.Invoke(isAllCompleted);
    }
}
