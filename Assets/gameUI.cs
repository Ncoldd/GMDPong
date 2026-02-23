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
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        winMessageText.gameObject.SetActive(false);

        startButton.onClick.AddListener(OnStartButtonClicked);
    }

    void OnStartButtonClicked()
    {
        gameManager.StartNewGame();
        startButton.gameObject.SetActive(false);
    }

    public override void OnNetworkSpawn()
    {
        gameManager = FindObjectOfType<GameManager>();
        gameManager.gameFinished.OnValueChanged += OnGameFinished;
    }

    void OnGameFinished(bool oldValue, bool newValue)
    {
        if (!newValue) return;

        winMessageText.gameObject.SetActive(true);

        if (gameManager.GetRightScore() >= gameManager.GetWinningScore())
            winMessageText.text = "Right Paddle Wins!";
        else
            winMessageText.text = "Left Paddle Wins!";
    }

}
