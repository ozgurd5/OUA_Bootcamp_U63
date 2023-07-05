using UnityEngine;

public class ElevatorParenter : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        if (!col.CompareTag("RedPuzzle") && !col.CompareTag("GreenPuzzle") && !col.CompareTag("BluePuzzle")) return;
        
        col.transform.SetParent(transform.parent);
    }

    //private void OnTriggerExit(Collider col)
    //{
    //    if (!col.CompareTag("RedPuzzle") && !col.CompareTag("GreenPuzzle") && !col.CompareTag("BluePuzzle")) return;
    //    
    //    col.transform.SetParent(null);
    //}
}
