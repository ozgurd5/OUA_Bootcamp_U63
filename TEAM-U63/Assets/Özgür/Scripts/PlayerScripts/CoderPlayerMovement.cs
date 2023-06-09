using Unity.Netcode;
using UnityEngine;

public class CoderPlayerMovement : NetworkBehaviour
{
    [SerializeField] private float speed;
    
    private float horizontalMove;
    private float verticalMove;
    
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false; //Weird bug fix
    }
    
    private void Update()
    {
        if (!IsHost) return;
        
        horizontalMove = Input.GetAxisRaw("Horizontal");
        verticalMove = Input.GetAxisRaw("Vertical");
        
        rb.velocity = new Vector3(horizontalMove * speed, rb.velocity.y, verticalMove * speed);
    }
}
