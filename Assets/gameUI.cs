using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using UnityEngine.UI;


public class gameUI : NetworkBehaviour
{
    public Button startButton;
    public TextMeshProUGUI winMessageText;
    private GameManager gameManager;


    // Start is called before the first frame update
    void Start() //on start
    {
        gameManager = FindObjectOfType<GameManager>();
        winMessageText.gameObject.SetActive(false);
        startButton.gameObject.SetActive(false);

        startButton.onClick.AddListener(OnStartButtonClicked);
    }

    void OnStartButtonClicked()
    {
        gameManager.StartNewGameServerRpc();
        startButton.gameObject.SetActive(false);
    }

    public override void OnNetworkSpawn()
    {
        gameManager = FindObjectOfType<GameManager>();
        gameManager.gameFinished.OnValueChanged += OnGameFinished;
    }

    void OnGameFinished(bool oldValue, bool newValue) //on end
    {
        if (newValue)
        {
            //when it is a new value turn these on
            winMessageText.gameObject.SetActive(true); //turned it back on
            startButton.gameObject.SetActive(true); //turned it back on 

            if (gameManager.GetRightScore() >= gameManager.GetWinningScore())
                winMessageText.text = "Right Paddle Wins!";
            else
                winMessageText.text = "Left Paddle Wins!";
        }
        else
        {
            winMessageText.gameObject.SetActive(false);
            startButton.gameObject.SetActive(false);
        }
    }

}
