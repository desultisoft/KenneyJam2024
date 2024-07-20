using UnityEngine;

public class HubPlayer : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer renderer;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    public bool canMove { get; private set; }

    public float moveSpeed = 5f;

    void Awake()
    {
        rb = GetComponentInChildren<Rigidbody2D>();
        renderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();

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

    public void SetCanMove(bool isOn)
    {
        canMove = isOn;
    }
}
