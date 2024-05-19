using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Island : MonoBehaviour
{
    public int coinCount, bulletCount, cannonBallsCount, medicineCount;
    public TextMeshProUGUI coins, bulletsButton, cannonBallsButton, medicineButton;

    private int _bulletPrice = 2, _cannonBallPrice = 2, _medicinePrice = 2;

    private void Start()
    {
        coins.text = "Coins: " + coinCount.ToString();
        bulletsButton.text = "Buy\nBullets\n " + _bulletPrice + " coins\nOwned: " + bulletCount.ToString();
        cannonBallsButton.text = "Buy\nCannon Balls\n " + _cannonBallPrice + " coins\nOwned: " + cannonBallsCount.ToString();
        medicineButton.text = "Buy\nMedicine\n " + _medicinePrice + " coins\nOwned: " + medicineCount.ToString();
    }

    public void SetCoins()
    {
        coinCount++;
        coins.text = "Coins: " + coinCount.ToString();
    }

    public void BuyBullets()
    {
        if (coinCount >= _bulletPrice)
        {
            bulletCount++;
            coinCount -= _bulletPrice;
            coins.text = "Coins: " + coinCount.ToString();
            bulletsButton.text = "Buy\nBullets\n " + _bulletPrice + " coins\nOwned: " + bulletCount.ToString();
        }
        else
        {
            Debug.Log("Too poor!");
        }
    }

    public void BuyCannonBalls()
    {
        if (coinCount >= _cannonBallPrice)
        {
            cannonBallsCount++;
            coinCount -= _cannonBallPrice;
            coins.text = "Coins: " + coinCount.ToString();
            cannonBallsButton.text = "Buy\nCannon Balls\n " + _cannonBallPrice + " coins\nOwned: " + cannonBallsCount.ToString();
        }
        else
        {
            Debug.Log("Too poor!");
        }
    }

    public void BuyMedicine()
    {
        if (coinCount >= _medicinePrice)
        {
            medicineCount++;
            coinCount -= _medicinePrice;
            coins.text = "Coins: " + coinCount.ToString();
            medicineButton.text = "Buy\nMedicine\n " + _medicinePrice + " coins\n Owned: " + medicineCount.ToString();
        }
        else
        {
            Debug.Log("Too poor!");
        }
    }
}
