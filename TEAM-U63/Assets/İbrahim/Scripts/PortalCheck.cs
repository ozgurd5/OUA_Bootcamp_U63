using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PortalCheck : MonoBehaviour
{   
    private int numberOfPlayers = 0;
    public int requiredPlayers = 2;


    private void Update()
    {
        if (requiredPlayers == numberOfPlayers)
        {
            Debug.Log("both players are in");

        }
        Debug.Log(numberOfPlayers);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            numberOfPlayers++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            numberOfPlayers--;
        }
    }
}