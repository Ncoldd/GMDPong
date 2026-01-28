using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(3f, 3f);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 v = rb.velocity;

        // Reverses Y direction when hitting top/bottom walls
        if (collision.gameObject.name.Contains("Top") ||
            collision.gameObject.name.Contains("Bottom"))
        {
            v.y = -v.y;
        }

        rb.velocity = v;
    }
}
