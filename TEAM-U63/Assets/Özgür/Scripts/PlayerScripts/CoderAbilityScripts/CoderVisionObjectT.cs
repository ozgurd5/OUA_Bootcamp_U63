using UnityEngine;

/// <summary>
/// <para>Assign this script to the objects that will be enabled when coder vision is activated</para>
/// <para>Unity doesn't accept the name CoderVisionObject so it's CoderVisionObjectT. Weird Unity problem</para>
/// </summary>
public class CoderVisionObjectT : MonoBehaviour
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
}