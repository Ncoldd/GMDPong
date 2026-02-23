using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ScoreZone : NetworkBehaviour
{

    public bool isLeftZone;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsServer) return;

        if (collision.CompareTag("Ball"))
        {
            Debug.Log("Ball hit the " + isLeftZone + " left zone");

            GameManager gm = FindObjectOfType<GameManager>();

            if (isLeftZone)
                gm.AddRightScoreServerRpc();
            else
                gm.AddLeftScoreServerRpc();
        }
    }

}

