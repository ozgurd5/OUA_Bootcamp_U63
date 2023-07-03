using UnityEngine;

public class PlayerGroundChecker : MonoBehaviour
{
    private PlayerStateData psd;
    
    private bool groundCheck;
    private float groundCheckBufferLimit = 0.05f;
    private float groundCheckBufferTimer;

    private void Awake()
    {
        psd = GetComponentInParent<PlayerStateData>();
    }

    private void Update()
    {
        psd.isGrounded = true;
        
        groundCheck = Physics.Raycast(transform.position, Vector3.down, 0.1f);

        if (groundCheck)
            groundCheckBufferTimer = groundCheckBufferLimit;
        
        else
        {
            groundCheckBufferTimer -= Time.deltaTime;

            if (groundCheckBufferTimer <= 0f)
            {
                psd.isGrounded = false;
                Debug.Log("hop");
            }
        }

        psd.isJumping = !psd.isGrounded;
    }
}
