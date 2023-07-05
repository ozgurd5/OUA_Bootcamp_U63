using UnityEngine;

/// <summary>
/// <para>Assign this script to the objects that will be enabled when coder vision is activated</para>
/// </summary>
public class CoderVisionObject : MonoBehaviour
{
    private MeshRenderer mr;

    private void Awake()
    {
        mr = GetComponent<MeshRenderer>();
        
        PlayerCoderVisionAbility.OnCoderVisionEnable += EnableMeshRenderer;
    }

    private void EnableMeshRenderer()
    {
        mr.enabled = PlayerCoderVisionAbility.isCoderVisionActive;
    }
}