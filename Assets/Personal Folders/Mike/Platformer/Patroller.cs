using UnityEngine;
using System.Collections;

public class Patroller : MonoBehaviour
{
    public float speed = 2f;
    public Transform groundDetection;
    public float groundCheckDistance = 2f;
    public LayerMask groundLayer;
    public float idleTime = 2f;
    private bool movingRight = true;
    public Animator animator;
    private bool isIdling = false;

    void Update()
    {
        if (!isIdling)
        {
            Patrol();
            animator.SetTrigger("Walk");
        }
        else
        {
            animator.SetTrigger("Idle");
        }
    }

    void Patrol()
    {
        // Move the enemy
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        // Ground detection
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, groundCheckDistance, groundLayer);
        if (groundInfo.collider == false)
        {
            // If no ground detected, turn around and idle
            if (movingRight)
            {

                transform.eulerAngles = new Vector3(0, -180, 0);
                movingRight = false;
                
            }
            else
            {

                transform.eulerAngles = new Vector3(0, 0, 0);
                movingRight = true;
            }

            StartCoroutine(Idle());
        }
    }

    IEnumerator Idle()
    {
        isIdling = true;
        yield return new WaitForSeconds(idleTime);
        isIdling = false;
    }
}
