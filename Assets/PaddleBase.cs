using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class PaddleBase : NetworkBehaviour
{
    public float speed = 5f;       // Movement speed
    protected Rigidbody2D rb;      // Rigidbody reference
    protected float moveInput;     // Current input

    private NetworkVariable<float> networkYPosition =
        new NetworkVariable<float>(); // Sync Y position across the network


    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        if (!IsOwner) return;

        // GetInput() will be defined in the derived class
        moveInput = GetInput();
    }

    protected virtual void FixedUpdate()
    {
        if (IsOwner)
        {
            rb.velocity = new Vector2(0, moveInput * speed);
            networkYPosition.Value = transform.position.y; // Update network variable
        }
        else
        {
            // For non-owners, update position based on network variable
            Vector3 newPosition = transform.position;
            newPosition.y = networkYPosition.Value;
            transform.position = newPosition;
        }
    }

    protected abstract float GetInput();

}
