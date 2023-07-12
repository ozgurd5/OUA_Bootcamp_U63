using UnityEngine;

/// <summary>
/// <para>Enables QTE ability in the end of the first island</para>
/// <para>Assign this script to the portal in the end of the first island</para>
/// </summary>
public class FirstIslandQTEActivator : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        PlayerQTEAbility.IsQTEActive = true;
    }
}