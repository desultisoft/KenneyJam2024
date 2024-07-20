using UnityEngine;

public class HubPlayer : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer myRenderer;
    private Rigidbody2D rb;
    private Vector2 moveInput;

    private bool autoMoveOn;
    private Vector3 autoMoveTarget;
    public bool canMove { get; private set; }

    public float moveSpeed = 5f;

    void Awake()
    {
        rb = GetComponentInChildren<Rigidbody2D>();
        myRenderer = GetComponentInChildren<SpriteRenderer>();
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

        myRenderer.flipX = rb.velocity.x < 0.1;
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            // Move the player using the Rigidbody2D
            rb.velocity = moveInput * moveSpeed;
        }
        else if(autoMoveOn)
        {
            Vector3 moveDir = (autoMoveTarget - transform.position);
            const float targetRange = 0.1f;
            rb.velocity = moveDir.normalized * moveSpeed;
            if (moveDir.magnitude < targetRange) autoMoveOn = false;
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

    public void SetAutoTarget(Vector3 targetPosition)
    {
        autoMoveTarget = targetPosition;
        autoMoveOn = true;
    }
}
