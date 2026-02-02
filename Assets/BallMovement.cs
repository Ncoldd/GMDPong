using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    //engine accessible speed
    public float speed = 5f;
    private Vector2 direction = new Vector2(1, 1);

    
    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    public Vector2 Direction
    {
        get { return direction; }
        set { direction = value.normalized; } // Always keep normalized
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction * speed; 
        // initial velocity
    }

    void FixedUpdate()
    {
        // updates move speed and direction
        rb.velocity = direction * speed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
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

}

