using UnityEngine;

public class AmogusPlayerController: MonoBehaviour
{
    [SerializeField] private float speed = 12f;

    private Rigidbody rb;
    private InputManager im;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        im = GetComponent<InputManager>();
    }
    
    private void Update()
    {
        rb.velocity = new Vector3(im.movementInput.x * speed, rb.velocity.y, im.movementInput.y * speed);
    }
}