using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAnimatorController : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rb;
    SpriteRenderer rend;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("xVelocity", Mathf.Abs(rb.velocity.x));

        if (rb.velocity.x > 0.1f)
        {
            rend.flipX = false;
        }
        else if (rb.velocity.x < -0.1f)
        {
            rend.flipX = true;
        }
    }
}
