using Unity.Netcode;
using UnityEngine;

public class ArtistPlayerMovement : NetworkBehaviour
{
    [SerializeField] private float speed = 12f;
    
    private NetworkInputManager.InputData currentInput;
    
    private Rigidbody rb;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    private void Update()
    {
        if (!IsHost) return;
        
        if (!NetworkData.isHostCoder.Value)
            currentInput = NetworkInputManager.hostInput;
        else
            currentInput = NetworkInputManager.clientInput;
        
        rb.velocity = new Vector3(currentInput.moveInput.x * speed, rb.velocity.y, currentInput.moveInput.y * speed);

        if (currentInput.isRotatingKey)
        {
            transform.Rotate(0f, 360f * Time.deltaTime, 0f);
        }
    }
}