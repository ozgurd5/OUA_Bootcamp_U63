using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoderVisionObject : MonoBehaviour
{
    public CoderVision coderVision;
    

    
    private void Start()
    {
        //coderVision = GetComponent<CoderVision>();
    }
    
    // Update is called once per frame
    void Update()
    {

        gameObject.SetActive(coderVision.isCoderVisionActive);
        
    }
    
    
}
