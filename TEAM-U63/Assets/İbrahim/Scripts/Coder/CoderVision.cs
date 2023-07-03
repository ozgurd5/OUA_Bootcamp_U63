using UnityEngine;

public class CoderVision : MonoBehaviour
{
    public bool isCoderVisionActive;
    public GameObject coderVisionOverlay;

    void Update()
    {
        if (Input.GetKeyDown("v"))
        {
            isCoderVisionActive = !isCoderVisionActive;
            coderVisionOverlay.SetActive(isCoderVisionActive);

        }
    }
}