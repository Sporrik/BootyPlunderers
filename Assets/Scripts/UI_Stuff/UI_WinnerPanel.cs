using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_WinnerPanel : MonoBehaviour
{
    public TextMeshProUGUI playerText, finalCoins;
    public GameObject winnerPirate, winnerPirateIcon;
    private int winner;  //just to get rid of errors

    public GameManager gameManager;

    private void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();

        winner = gameManager.player1.coins - gameManager.player2.coins;

        if (winner > 0) //player 1 is the winner
        {
            playerText.text = "Player 1";
            finalCoins.text = "";//coins gained by player 1
            winnerPirateIcon.transform.GetChild(0).gameObject.SetActive(true);
            winnerPirateIcon.transform.GetChild(1).gameObject.SetActive(false);
        }
        else if (winner <= 0) //player 2 is the winner
        {
            playerText.text = "Player 2";
            finalCoins.text = "";//coins gained by player 2
            winnerPirateIcon.transform.GetChild(1).gameObject.SetActive(true);
            winnerPirateIcon.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
