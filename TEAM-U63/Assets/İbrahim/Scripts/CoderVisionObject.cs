using UnityEngine;

public class CoderVisionObject : MonoBehaviour
{
    public CoderVision coderVision;

    private MeshRenderer mr;

    private void Awake()
    {
        mr = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        mr.enabled = coderVision.isCoderVisionActive;
    }
}