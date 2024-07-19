using UnityEngine;

public class HubPlayer : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector2 moveInput;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
    }

    void FixedUpdate()
    {
        // Move the player using the Rigidbody2D
        rb.velocity = moveInput * moveSpeed;
    }
}
