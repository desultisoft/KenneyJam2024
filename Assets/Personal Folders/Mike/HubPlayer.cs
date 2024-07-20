using UnityEngine;

public class HubPlayer : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer renderer;

    public float moveSpeed = 5f;
    private Vector2 moveInput;
    private Rigidbody2D rb;
    internal bool canMove;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        canMove = true;
    }

    void Update()
    {
        
        // Get input from arrow keys
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        // Normalize the movement vector to ensure consistent speed in all directions
        if (moveInput != Vector2.zero)
        {
            moveInput = moveInput.normalized;
        }
        float xVelocity = Mathf.Abs(rb.velocity.x);
        animator.SetFloat("xVelocity", xVelocity);

        renderer.flipX = rb.velocity.x < 0.1;

    }

    void FixedUpdate()
    {
        if (canMove)
        {
            // Move the player using the Rigidbody2D
            rb.velocity = moveInput * moveSpeed;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
       
    }
}
