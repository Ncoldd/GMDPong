using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleBase : MonoBehaviour
{
    public float speed = 5f;       // Movement speed
    protected Rigidbody2D rb;      // Rigidbody reference
    protected float moveInput;     // Current input

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        // GetInput() will be defined in the derived class
        moveInput = GetInput();
    }

    protected virtual void FixedUpdate()
    {
        rb.velocity = new Vector2(0, moveInput * speed);
    }

    // Each paddle defines its own input keys
    protected virtual float GetInput()
    {
        return Input.GetAxis("LeftPaddle");
    }
}
