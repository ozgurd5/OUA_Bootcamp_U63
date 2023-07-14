using System;
using UnityEngine;

public class LaserTarget : MonoBehaviour
{
    //TODO: don't make it static after bootcamp and scale too but scale complicated
    public static event Action<bool> OnLaserTargetHit;
    
    [Header("Assign - Material Index - RGB")]
    [SerializeField] private int targetMaterialIndex;

    //
    //
    private void Awake()
    {
        OnLaserTargetHit += b => { Debug.Log("correct hit"); };
    }

    public void CheckLaserTargetHit(int hittingRobotMaterialIndex)
    {
        //TODO: this logic can be carried to robot laser manager, design choice, so decide
        
        //Red (0) robot can only activate green (1) target, green (1) robot can only activate blue (2) target..
        //..and blue (2) robot can only activate red (0) target
        if (hittingRobotMaterialIndex + 1 == targetMaterialIndex) //R -> G -> B
            OnLaserTargetHit?.Invoke(true);
        else if (hittingRobotMaterialIndex - 2 == targetMaterialIndex) //B -> R
            OnLaserTargetHit?.Invoke(true);
        else
            OnLaserTargetHit?.Invoke(false);
    }
}