using UnityEngine;

public class PlayerJumpAbilitySwitch : MonoBehaviour
{
    private PlayerStateData psd;

    private void Awake()
    {
        psd = GetComponent<PlayerStateData>();
    }

    private void MakePlayerCanJump()
    {
        psd.canJump = true;
    }

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
