using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParenter : MonoBehaviour
{
    private Transform elevator;
    private void Awake()
    {
        elevator = transform.parent;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(elevator);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(null);
        }
    }
}
