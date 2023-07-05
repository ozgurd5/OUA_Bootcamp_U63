using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    
        [SerializeField] private float colorChangeRange = 5.0f;
    
        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, colorChangeRange))
                {
                    GameObject hitObject = hit.collider.gameObject;
                    MeshRenderer meshRenderer = hitObject.GetComponent<MeshRenderer>();
    
                    if (hitObject.CompareTag("RedPuzzle"))
                    {
                        meshRenderer.material.color = Color.blue;
                        hitObject.tag = "BluePuzzle";
                    }
                    else if (hitObject.CompareTag("BluePuzzle"))
                    {
                        meshRenderer.material.color = Color.green;
                        hitObject.tag = "GreenPuzzle";
                    }
                    else if (hitObject.CompareTag("GreenPuzzle"))
                    {
                        meshRenderer.material.color = Color.red;
                        hitObject.tag = "RedPuzzle";
                    }
                }
            }
        }
    

}
