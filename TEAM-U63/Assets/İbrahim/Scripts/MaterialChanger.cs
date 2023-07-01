using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialChanger : MonoBehaviour
{
    

    public float materialChangeRange = 10f;

    private int currentMaterialIndex = 0;
    private int currentTagIndex = 0;

    private string[] puzzlesTag = new string[] { "RedPuzzle", "GreenPuzzle", "BluePuzzle" };

    public List<Material> puzzleMaterials;
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
            if (Physics.Raycast(ray, out hit, materialChangeRange))
            {
                GameObject hitObject = hit.collider.gameObject;

                if (hitObject.CompareTag("RedPuzzle") || hitObject.CompareTag("GreenPuzzle") || hitObject.CompareTag("BluePuzzle"))
                {
                    // Change the material of the puzzle piece
                    MeshRenderer puzzleMeshRenderer = hitObject.GetComponent<MeshRenderer>();
                    if (puzzleMeshRenderer != null)
                    {
                        currentMaterialIndex = (currentMaterialIndex + 1) % puzzleMaterials.Count;
                        Material[] materials = puzzleMeshRenderer.materials;
                        materials[0] = puzzleMaterials[currentMaterialIndex];
                        puzzleMeshRenderer.materials = materials;
                    }
                

                    // Change the tag of the hit object
                    currentTagIndex = (currentTagIndex + 1) % puzzlesTag.Length;
                    hitObject.tag = puzzlesTag[currentTagIndex];
                }
            }
        }
    }
}