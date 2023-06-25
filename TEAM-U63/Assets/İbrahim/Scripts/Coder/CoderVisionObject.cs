using UnityEngine;

public class CoderVisionObject : MonoBehaviour
{
    public CoderVision coderVision;

    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        meshRenderer.enabled = coderVision.isCoderVisionActive;
    }
}