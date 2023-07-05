using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimControl : MonoBehaviour
{
    [SerializeField] public GameObject blinkEyesObject;
    [SerializeField] public GameObject hypnotizedEyesObject;
    [SerializeField] public GameObject quickTimeEventCanvas;
    
    private FlyingRobotController _flyingRobotController;

    public void Awake()
    {
         _flyingRobotController = GetComponent<FlyingRobotController>();
    }

    private void Update()
    {
        if (quickTimeEventCanvas.activeSelf || _flyingRobotController.enabled)
        {
            blinkEyesObject.SetActive(false);
            hypnotizedEyesObject.SetActive(true);
        }
        else
        {
            blinkEyesObject.SetActive(true);
            hypnotizedEyesObject.SetActive(false);
        }
        
        
        
    }
}
