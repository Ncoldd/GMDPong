using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using TMPro;

public class GameManager : NetworkBehaviour
{
//    // Start is called before the first frame update
//    void Start()
//    {
//    }

    public TextMeshProUGUI leftScoreText;
    public TextMeshProUGUI rightScoreText;

    public NetworkVariable<int> leftScore = new NetworkVariable<int>(0);
    public NetworkVariable<int> rightScore = new NetworkVariable<int>(0);
    public NetworkVariable<bool> gameFinished = new NetworkVariable<bool>(false);
    public NetworkVariable<int> winningScore = new NetworkVariable<int>(10);

    //Gets mandatory for network vars
    public int GetLeftScore()
    { return leftScore.Value; }

    public int GetRightScore()
    { return rightScore.Value; }

    public int GetWinningScore()
    { return winningScore.Value; }

    public override void OnNetworkSpawn()
    {
        leftScore.OnValueChanged += OnScoreChanged;
        rightScore.OnValueChanged += OnScoreChanged;

        UpdateUI();

        if (!IsServer) return;

        NetworkManager.Singleton.OnClientConnectedCallback += AssignPaddles;
    }

    void AssignPaddles(ulong clientId)
    {
        PaddleBase[] paddles = FindObjectsOfType<PaddleBase>();
        ulong hostId = NetworkManager.Singleton.LocalClientId;

        foreach (PaddleBase paddle in paddles)
        {
            NetworkObject netObj = paddle.GetComponent<NetworkObject>();

            if (paddle.transform.position.x < 0)
            {
                // LEFT - give to client 
                if (clientId != hostId)
                {
                    netObj.ChangeOwnership(clientId);
                }
            }
            else
            {
                // RIGHT - always host
                netObj.ChangeOwnership(hostId);
            }
        }
    }

    void OnScoreChanged(int oldValue, int newValue)
    {
        UpdateUI();
    }

    // Update is called once per frame
    void UpdateUI()
    {
        leftScoreText.text = leftScore.Value.ToString();
        rightScoreText.text = rightScore.Value.ToString();
    }

    [ServerRpc(RequireOwnership = false)]
    public void AddLeftScoreServerRpc()
    {   //!gameFinished.Value prevents this from running
        if (IsServer && !gameFinished.Value)
        {
            leftScore.Value++;
            CheckWinCondition();
            ResetBall(Vector2.right); //goes towards the right player
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void AddRightScoreServerRpc()
    {   //!gameFinished.Value prevents this from running
        if (IsServer && !gameFinished.Value)
        { 
            rightScore.Value++;
            CheckWinCondition();
            ResetBall(Vector2.left); //goes towards the left player
        } 
    }

    [ServerRpc(RequireOwnership = false)]
    public void StartNewGameServerRpc()
    {
        StartNewGame();
    }

    void CheckWinCondition()
    {
        if (rightScore.Value >= winningScore.Value)
        {
            gameFinished.Value = true;

            StopBall();

            Debug.Log("---------------Right Paddle wins---------------");
        }
        else if (leftScore.Value >= winningScore.Value)
        {
            gameFinished.Value = true; 

            StopBall();

            Debug.Log("---------------Left Paddle wins---------------");
        }
    }

    void StopBall()
    {

        //locates game object or objects with "BallMovement" script attached 
        BallMovement ball = FindObjectOfType<BallMovement>();

        //grabs its Rigidbody2D component
        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();

        rb.velocity = Vector2.zero; //makes it stop permenantly
        ball.direction = Vector2.zero;  //makes it stop permenantly

        ball.enabled = false;

        
        //PaddleBase paddle = FindObjectOfType<PaddleBase>();
        //Rigidbody2D rbp = paddle.GetComponent<Rigidbody2D>();

        //paddle.enabled = false;

    }

    public void StartNewGame()
    {
        rightScore.Value = 0;
        leftScore.Value = 0;
        gameFinished.Value = false;
        IsGameFinished();;


        BallMovement ball = FindObjectOfType<BallMovement>();

        ball.enabled = true;

        ResetPaddlesPos();

        Debug.Log("New game started");
    }


    public bool IsGameFinished()
    {
        return gameFinished.Value;
    }

    void ResetPaddlesPos()
    {
        PaddleBase[] paddles = FindObjectsOfType<PaddleBase>();

        foreach (PaddleBase paddle in paddles)
        {
            if (paddle.transform.position.x < 0)
            {
                paddle.transform.position = new Vector3(-7.98f, 0f, 0f);
            }
            else
            {
                paddle.transform.position = new Vector3(7.98f, 0f, 0f);
            }
        }
    }


    void ResetBall(Vector2 direction)
    {
        BallMovement ball = FindObjectOfType<BallMovement>();
        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();

        //resets pos to 0
        ball.transform.position = Vector2.zero;

        //freeze pos to that 0
        rb.velocity = Vector2.zero;

        ball.direction = direction;
    }

}
