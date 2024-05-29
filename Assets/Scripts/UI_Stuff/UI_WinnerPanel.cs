using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_WinnerPanel : MonoBehaviour
{
    public TextMeshProUGUI playerText, finalCoins;
    public GameObject winnerPirate, winnerPirateIcon;

    private void Start()
    {
        if () //player 1 is the winner
        {
            playerText.text = "Player 1";
            finalCoins.text = ;//coins gained by player 1
            winnerPirateIcon.transform.GetChild(0).gameObject.SetActive(true);
            winnerPirateIcon.transform.GetChild(1).gameObject.SetActive(false);
        }
        else if () //player 2 is the winner
        {
            playerText.text = "Player 2";
            finalCoins.text = ;//coins gained by player 2
            winnerPirateIcon.transform.GetChild(1).gameObject.SetActive(true);
            winnerPirateIcon.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
