using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ElevatorParenter : NetworkBehaviour
{
    [Header("Assign - NetworkParentListID")]
    [SerializeField] private int networkParentListID;

    [Header("Info - No Touch")] [SerializeField] private List<CubeManager> enteredCubes;

    private void OnTriggerEnter(Collider col)
    {
        if (!col.CompareTag("RedPuzzle") && !col.CompareTag("GreenPuzzle") && !col.CompareTag("BluePuzzle")) return;
        
        //Updating elevator position is not smooth and looks very bad while player is holding the cube
        //We can not check if player is holding the cube in OnTriggerEnter method. Player can drop..
        //..the cube after it's entry to the collider. So we must check it dynamically in FixedUpdate
        
        //Also the parenting in this script cause conflict with PlayerGrabbing.cs parenting. It must also be
        //..done while player is not holding the cube.
        
        //To do all of that, we need the CubeManager.cs from the cube that has entered the collider...
        //..and check if it's currently held by player or not
        enteredCubes.Add(col.GetComponent<CubeManager>());
    }

    private void OnTriggerExit(Collider col)
    {
        enteredCubes.Remove(col.GetComponent<CubeManager>());
    }

    private void Update()
    {
        foreach (var item in enteredCubes)
        {
            if (item.isGrabbed || item.isParented || item.isRotationFroze) return;

            //Parenting is needed for smooth movement and good looking motion
            item.ToggleFakeParenting(true, transform); //UpdateParentUsingNetworkParentListID(networkParentListID);
            item.UpdateRotationFreeze(true);
        }
    }
}
