using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePoint : MonoBehaviour
{
    public GameObject puzzlePiece;

    private void OnTriggerStay(Collider other)
    {
        if (puzzlePiece)
        {
            Debug.Log( "staying");
        }
        else
        {
            Debug.Log( "left");
        }
    }
}
