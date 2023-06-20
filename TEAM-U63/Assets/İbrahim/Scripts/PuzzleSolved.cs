using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PuzzleSolved : MonoBehaviour
{
    public int redPieceNumber;
    public int greenPieceNumber;
    public int bluePieceNumber;
    
    
    
    
    public GameObject targetObject;

    private Animator _animator;

    private void Start()
    {
        //_animator = targetObject.GetComponent<Animator>();

        

    }

    private void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RedPuzzle"))
        {
            redPieceNumber++;
            Debug.Log(redPieceNumber);
        }
        if (other.CompareTag("BluePuzzle"))
        {
            bluePieceNumber++;
        }
        if (other.CompareTag("GreenPuzzle"))
        {
            greenPieceNumber++;
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("RedPuzzle"))
        {
            redPieceNumber--;
        }
        if (other.CompareTag("BluePuzzle"))
        {
            bluePieceNumber--;
        }
        if (other.CompareTag("GreenPuzzle"))
        {
            greenPieceNumber--;
        }    }
}
