using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Island : MonoBehaviour
{
    public int coinCount;
    public TextMeshProUGUI coins;

    public void SetCoins()
    {
        coinCount++;
        coins.text = "Coins: " + coinCount.ToString();
    }
}
