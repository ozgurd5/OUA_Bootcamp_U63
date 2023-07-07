using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimControl : MonoBehaviour
{
    [SerializeField] public GameObject blinkEyesObject;
    [SerializeField] public GameObject hypnotizedEyesObject;
    [SerializeField] public GameObject hypnotizedEarsObject;
    [SerializeField] public GameObject normalEarsObject;
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
            normalEarsObject.SetActive(false);
            hypnotizedEarsObject.SetActive(true);
            hypnotizedEyesObject.SetActive(true);
        }
        else if(!quickTimeEventCanvas.activeSelf || !_flyingRobotController.enabled)
        {
            blinkEyesObject.SetActive(true);
            normalEarsObject.SetActive(true);
            hypnotizedEarsObject.SetActive(false);
            hypnotizedEyesObject.SetActive(false);
        }
    }
}
