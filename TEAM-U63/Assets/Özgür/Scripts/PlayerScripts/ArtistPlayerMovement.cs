using Unity.Netcode;
using UnityEngine;

public class ArtistPlayerMovement : NetworkBehaviour
{
    [SerializeField] private float speed = 12f;

    private NetworkInputManager nim;
    private Rigidbody rb;

    private void Awake()
    {
        nim = NetworkInputManager.Singleton;
        rb = GetComponent<Rigidbody>();
    }
    
    private void Update()
    {
        if (!IsHost) return;

        rb.velocity = new Vector3(nim.artistInput.moveInput.x * speed, rb.velocity.y, nim.artistInput.moveInput.y * speed);

        if (nim.artistInput.isRotatingKey)
        {
            transform.Rotate(0f, 360f * Time.deltaTime, 0f);
        }
    }
}