using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotPainter : MonoBehaviour
{
    

    public float paintRange = 10f;

    private int currentMaterialIndex = 0;
    

    

    public List<Material> robotMaterials;
    //private LayerMask puzzlePieceLayerMask;

    //private void Start()
    //{
    //    puzzlePieceLayerMask = LayerMask.GetMask("puzzlePieceLayerMask");
    //}

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, paintRange))
            {
                GameObject hitObject = hit.collider.gameObject;

                if (hitObject.CompareTag("robot"))
                {
                    // Change the material of the puzzle piece
                    MeshRenderer robotMeshRenderer = hitObject.GetComponent<MeshRenderer>();
                    
                    
                        currentMaterialIndex = (currentMaterialIndex + 1) % robotMaterials.Count;
                        Material[] materials = robotMeshRenderer.materials;
                        materials[0] = robotMaterials[currentMaterialIndex];
                        robotMeshRenderer.materials = materials;
                    
                }
                
                

                
            }
        }
    }
}
