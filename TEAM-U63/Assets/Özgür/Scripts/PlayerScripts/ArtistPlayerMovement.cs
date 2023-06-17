using Unity.Netcode;
using UnityEngine;

//This line is the reason why we can use artistInput without referencing NetworkInputManager
using static NetworkInputManager;

public class ArtistPlayerMovement : NetworkBehaviour
{
    [SerializeField] private float speed = 12f;

    private Rigidbody rb;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    private void Update()
    {
        if (!IsHost) return;

        rb.velocity = new Vector3(artistInput.moveInput.x * speed, rb.velocity.y, artistInput.moveInput.y * speed);

        if (artistInput.isRotatingKey)
        {
            transform.Rotate(0f, 360f * Time.deltaTime, 0f);
        }
    }
}