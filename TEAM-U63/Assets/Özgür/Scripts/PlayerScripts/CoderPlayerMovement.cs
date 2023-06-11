using Unity.Netcode;
using UnityEngine;

//SERVER SIDE

//This script controls coder in server side

public class CoderPlayerMovement : NetworkBehaviour
{
    [SerializeField] private float speed = 12f;
    
    private float horizontalMove;
    private float verticalMove;
    
    private Rigidbody rb;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    private void Update()
    {
        if (!IsHost) return;
        
        horizontalMove = Input.GetAxisRaw("Horizontal");
        verticalMove = Input.GetAxisRaw("Vertical");
        
        rb.velocity = new Vector3(horizontalMove * speed, rb.velocity.y, verticalMove * speed);
        
        //
        Debug.Log("coder: " + transform.position);
    }
}
