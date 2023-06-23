using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateArtistLullaby : MonoBehaviour
{
    
    public GameObject artistCanvas;

    private void Update()
    {
        if (Input.GetKeyDown("h"))
        {
            ActivateArtist();
            Debug.Log("activated");
        }
    }
    
    private void ActivateArtist()
    {
        artistCanvas.SetActive(true);
    }
}
