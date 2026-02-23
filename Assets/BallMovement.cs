using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor.Build.Content;
using UnityEngine;

public class BallMovement : NetworkBehaviour, ICollidable
{
    private Rigidbody2D rb;
    public float speed = 5f;
    public Vector2 direction = new Vector2(1, 1);

    public void ResetBall()
    {
        transform.position = Vector2.zero;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }
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
            float angle = Random.Range(-0.7f, 0.7f);
            direction.y += angle;
            direction = direction.normalized;

            speed = Random.Range(5f, 10f);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsServer) return;

        if (collision.CompareTag("LeftScoreZone"))
        {
            FindObjectOfType<GameManager>().AddRightScoreServerRpc();
            ResetBall();
        }
        else if (collision.CompareTag("RightScoreZone"))
        {
            FindObjectOfType<GameManager>().AddLeftScoreServerRpc();
            ResetBall();
        }

    }
}
