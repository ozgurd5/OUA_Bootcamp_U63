using UnityEngine;

public class ElevatorParenter : MonoBehaviour
{
    private CubeStateManager enteredCubeStateManager;   //Explanation is in further down where it's being used
    private bool isEnteredCubeStatesNull = true;        //Comparison to null is expensive
    
    private void OnTriggerEnter(Collider col)
    {
        if (!col.CompareTag("RedPuzzle") && !col.CompareTag("GreenPuzzle") && !col.CompareTag("BluePuzzle")) return;
        
        //Updating scale position is not smooth and looks very bad while player is holding the cube
        //We can not check if player is holding the cube in OnTriggerEnter method. Player can drop..
        //..the cube after it's entry to the collider. So we must check it dynamically in FixedUpdate
        
        //Also the parenting in this script cause conflict with PlayerGrabbing.cs parenting. It must also be
        //..done while player is not holding the cube.
        
        //To do all of that, we need the CubeStateManager.cs from the cube that has entered the collider...
        //..and check if it's currently held by player or not
        enteredCubeStateManager = col.GetComponent<CubeStateManager>();
        isEnteredCubeStatesNull = false;    //Comparison to null is expensive, we will check that variable instead
    }
    
    private void FixedUpdate()
    {
        if (isEnteredCubeStatesNull) return; 
        if (enteredCubeStateManager.isGrabbed) return;
        
        //Parenting is needed for smooth movement and good looking motion
        enteredCubeStateManager.transform.SetParent(transform.parent);

        enteredCubeStateManager = null;
        isEnteredCubeStatesNull = true; //Comparison to null is expensive, we will check that variable instead
    }
}
