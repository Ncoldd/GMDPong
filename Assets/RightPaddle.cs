using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightPaddle : PaddleBase, ICollidable
{
    

    protected override float GetInput()
    {
        return Input.GetAxis("RightPaddle"); // up down
    }

    public void OnHit(Collision2D collision)
    {
        // llallallallallallallallallallllalla
        // ball will check this when collided
        GetComponent<SpriteRenderer>().color = Color.red;
        Invoke(nameof(ResetColor), 0.1f);
    }

    void ResetColor()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }

}
