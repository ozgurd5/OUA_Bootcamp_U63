using UnityEngine;

/// <summary>
/// <para>Assign this script to the objects that will be enabled when coder vision is activated</para>
/// <para>Since PlayerCoderVisionAbility.cs works locally, this too works locally</para>
/// </summary>
public class CoderVisionObject : MonoBehaviour
{
    private MeshRenderer mr;

    private void Awake()
    {
        mr = GetComponent<MeshRenderer>();
        PlayerCoderVisionAbility.OnCoderVisionEnable += EnableMeshRenderer;
    }

    /// <summary>
    /// <para>Enables mesh renderer so that coder can see it</para>
    /// </summary>
    private void EnableMeshRenderer()
    {
        mr.enabled = PlayerCoderVisionAbility.isCoderVisionActive;
    }

    private void OnDestroy()
    {
        PlayerCoderVisionAbility.OnCoderVisionEnable -= EnableMeshRenderer;
    }
}