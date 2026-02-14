using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BallMovement : NetworkBehaviour, ICollidable
{
    private Rigidbody2D rb;
    public float speed = 5f;
    private Vector2 direction = new Vector2(1, 1);

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (IsServer)
        {
            rb.velocity = direction * speed;
        }
    }

    void FixedUpdate()
    {
        // Only the server controls the ball
        if (!IsServer) return;

        rb.velocity = direction * speed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Only the server handles collisions
        if (!IsServer) return;

        // Reverse direction on collisions
        if (collision.gameObject.CompareTag("Paddle"))
        {
            direction.x = -direction.x;
            float angle = Random.Range(-0.2f, 0.2f);
            direction.y += angle;
            direction = direction.normalized;
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            direction.y = -direction.y;
        }
        else if (collision.gameObject.CompareTag("SideWall"))
        {
            direction.x = -direction.x;
        }
    }

    public void OnHit(Collision2D collision)
    {
    }
}
