using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PuzzleSolved : MonoBehaviour
{
    public int requiredRedPieces;
    public int requiredGreenPieces;
    public int requiredBluePieces;
        
    private int redPieceNumber;
    private int greenPieceNumber;
    private int bluePieceNumber;
    
    
    
    
    public GameObject targetObject;

    private Animator animator;

    private void Start()
    {
        animator = targetObject.GetComponent<Animator>();

        

    }

    private void Update()
    {
        if (requiredRedPieces == redPieceNumber && requiredBluePieces == bluePieceNumber && requiredGreenPieces == greenPieceNumber)
        {
            animator.SetBool("ButtonPressed", true);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RedPuzzle"))
        {
            redPieceNumber++;
            
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
