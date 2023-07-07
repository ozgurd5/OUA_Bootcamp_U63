using UnityEngine;

public class IboCoderVisionObject : MonoBehaviour
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