using System;
using System.Collections.Generic;
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

    [Header("Number of Cubes")]
    [SerializeField] private int redNumber;
    [SerializeField] private int greenNumber;
    [SerializeField] private int blueNumber;
    
    [Header("Info - No Touch")]
    public bool isCompleted;
    [SerializeField] private List<Collider> currentCubesInside;
    [SerializeField] private int currentCubeNumber;
    [SerializeField] private CubeManager currentCubeManager;

    private AudioSource aus;

    private void Awake()
    {
        aus = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (IsRobot && col.CompareTag("robot"))
        {
            isCompleted = true;
            completionStatus[ID] = true;
            CheckAllCompletion();
        }
        
        else if (col.CompareTag("RedPuzzle") || col.CompareTag("GreenPuzzle") || col.CompareTag("BluePuzzle"))
        {
            currentCubeManager = col.GetComponent<CubeManager>();
            currentCubeManager.OnCubePainted += CheckCubeCompletion;

            currentCubeNumber++;
            currentCubesInside.Add(col);
            CheckCubeCompletion();
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

        else if (col.CompareTag("RedPuzzle") || col.CompareTag("GreenPuzzle") || col.CompareTag("BluePuzzle"))
        {
            currentCubeManager = col.GetComponent<CubeManager>();
            currentCubeManager.OnCubePainted -= CheckCubeCompletion;
            
            currentCubeNumber--;
            currentCubesInside.Remove(col);
            CheckCubeCompletion();
        }
    }

    public void CheckCubeCompletion()
    {
        redNumber = 0;
        greenNumber = 0;
        blueNumber = 0;
        
        foreach (Collider cube in currentCubesInside)
        {
            if (cube.CompareTag("RedPuzzle")) redNumber++;
            if (cube.CompareTag("GreenPuzzle")) greenNumber++;
            if (cube.CompareTag("BluePuzzle")) blueNumber++;
        }

        isCompleted = redNumber == neededRed && greenNumber == neededGreen && blueNumber == neededBlue;
        completionStatus[ID] = isCompleted;
        CheckAllCompletion();
    }

    //TODO: FIND BETTER SOLUTION
    private void CheckAllCompletion()
    {
        isAllCompleted = completionStatus[0] && completionStatus[1] && completionStatus[2] && completionStatus[3];
        OnLevelCompleted?.Invoke(isAllCompleted);

        if (isAllCompleted) aus.Play();
    }
}
