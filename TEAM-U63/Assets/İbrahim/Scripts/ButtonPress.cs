using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPress : MonoBehaviour
{
    
    public GameObject targetObject;
    
    private Animator animator;
    

    private void Start()
    {
        animator = targetObject.GetComponent<Animator>();
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("playeris in");
            if (Input.GetKeyDown("e") && !animator.GetBool("ButtonPressed"))
            {
                animator.SetBool("ButtonPressed", true);
                Debug.Log("button pressed");
            }
            else if(Input.GetKeyDown("e") && animator.GetBool("ButtonPressed"))
            {
                animator.SetBool("ButtonPressed", false);
                Debug.Log("button pressed again");
            }
        }
    }
}
