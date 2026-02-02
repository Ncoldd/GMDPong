using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightPaddle : PaddleBase
{
    protected override float GetInput()
    {
        return Input.GetAxis("RightPaddle"); // up down
    }
}
