using UnityEngine;

public class CubeMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    
    private Rigidbody rb;
    private bool isGoingRight;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (isGoingRight)
            rb.velocity = Vector3.right * speed;
        else
            rb.velocity = Vector3.left * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Wall"))
            isGoingRight = !isGoingRight;
    }
}
