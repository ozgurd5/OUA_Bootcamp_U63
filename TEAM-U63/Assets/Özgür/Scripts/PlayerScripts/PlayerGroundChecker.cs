using UnityEngine;

public class PlayerGroundChecker : MonoBehaviour
{
    private PlayerStateData psd;

    private void Awake()
    {
        psd = GetComponentInParent<PlayerStateData>();
    }

    private void OnTriggerStay(Collider col)
    {
        psd.isGrounded = true;
        psd.isJumping = false;
    }

    private void OnTriggerExit(Collider col)
    {
        psd.isGrounded = false;
        psd.isJumping = true;
    }
}
