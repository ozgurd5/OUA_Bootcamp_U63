using UnityEngine;

/// <summary>
/// <para>Switches if the player can jump or not</para>
/// </summary>
public class PlayerJumpAbilitySwitch : MonoBehaviour
{
    private PlayerStateData psd;

    private void Awake()
    {
        psd = GetComponent<PlayerStateData>();
    }

    /// <summary>
    /// <para>Makes the player can jump</para>
    /// </summary>
    private void MakePlayerCanJump()
    {
        psd.canJump = true;
    }

    /// <summary>
    /// <para>Makes the player can not jump</para>
    /// </summary>
    private void MakePlayerCanNotJump()
    {
        psd.canJump = false;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("MakePlayerCanJump"))
            MakePlayerCanJump();
        else if (col.CompareTag("MakePlayerCanNotJump"))
            MakePlayerCanNotJump();
    }
}
