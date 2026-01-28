using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleMovement : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        float move = Input.GetAxis("Vertical");
        transform.position += Vector3.up * move * speed * Time.deltaTime;
    }
  
}
