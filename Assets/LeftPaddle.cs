using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftPaddle : PaddleBase
{
    protected override float GetInput()
    {
        return Input.GetAxis("LeftPaddle"); // w s
    }
}


