using Unity.Netcode;
using UnityEngine;

public class ArtistPlayerMovement : NetworkBehaviour
{
    [SerializeField] private float speed = 12f;
    
    private float horizontalInput;
    private float verticalInput;
    private bool isRotateKey;
    
    private Rigidbody rb;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    private void Update()
    {
        if (!NetworkData.isHostCoder.Value)
        {
            horizontalInput = HostInput.horizontalInput;
            verticalInput = HostInput.verticalInput;
            isRotateKey = HostInput.isRotateKey;
        }
        
        else
        {
            horizontalInput = ClientInput.horizontalInput;
            verticalInput = ClientInput.verticalInput;
            isRotateKey = ClientInput.isRotateKey;
        }
        
        rb.velocity = new Vector3(horizontalInput * speed, rb.velocity.y, verticalInput * speed);

        if (isRotateKey)
        {
            transform.Rotate(0f, 360f * Time.deltaTime, 0f);
        }
    }
}