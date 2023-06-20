using UnityEngine;

public class CoderVision : MonoBehaviour
{
    public bool isCoderVisionActive;

    void Update()
    {
        if (Input.GetKeyDown("v"))
        {
            isCoderVisionActive = !isCoderVisionActive;
        }
    }
}
