using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoderVision : MonoBehaviour
{

    public bool isCoderVisionActive;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("v"))
        {
            isCoderVisionActive = !isCoderVisionActive;
            Debug.Log(isCoderVisionActive);
        }
    }

    
}
