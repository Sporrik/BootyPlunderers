using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Island : MonoBehaviour
{    
    public int coinCount, bulletCount, cannonBallsCount, medicineCount;
    public TextMeshProUGUI coins, bulletsButton, cannonBallsButton, medicineButton, monkeyButton, parrotButton, boozeButton, healButton;

    public TextMeshProUGUI continueButton;

    public GameObject monkeyGuy, parrotGuy, boozeGuy, firstMate;

    public int crewHealth = 1, maxHealth = 100;

    private int _bulletPrice = 2, _cannonBallPrice = 2, _medicinePrice = 2, _healPrice = 2;

    private void Start()
    {
        firstMate = monkeyGuy; //change later
        SetButtonText();
        SetFirstMateButtons();
    }

    public void HealCrew()
    {
        if (crewHealth < maxHealth && coinCount >= _healPrice)
        {
            coinCount -= _healPrice;
            crewHealth = maxHealth;
            healButton.text = "Heal\nCrew\n[Healed]";
        }
        else if (crewHealth == maxHealth)
        {
            Debug.Log("The crew's health is full!");
        }   
        else if(coinCount < _healPrice)
        {
            Debug.Log("Too poor!");
        }
    }

    private void SetFirstMateButtons()
    {
        if (firstMate == monkeyGuy)
        {
            monkeyButton.text = "Monkey\nGuy\n[Selected]";
            parrotButton.text = "Parrot\nGuy";
            boozeButton.text = "Booze\nGuy\n";
        }
        else if (firstMate == parrotGuy)
        {
            parrotButton.text = "Parrot\nGuy\n[Selected]";
            monkeyButton.text = "Monkey\nGuy";
            boozeButton.text = "Booze\nGuy\n";
        }
        else if (firstMate == boozeGuy)
        {
            boozeButton.text = "Booze\nGuy\n[Selected]";
            parrotButton.text = "Parrot\nGuy";
            monkeyButton.text = "Monkey\nGuy";
        }
    }

    public void SetFirstMateMonkey()
    {
        if (firstMate != monkeyGuy)
        {
            firstMate = monkeyGuy;
            SetFirstMateButtons();
        }
        else
        {
            Debug.Log("This is already the first mate!");
        }
    }

    public void SetFirstMateParrot()
    {
        if (firstMate != parrotGuy)
        {
            firstMate = parrotGuy;
            SetFirstMateButtons();
        }
        else
        {
            Debug.Log("This is already the first mate!");
        }
    }

    public void SetFirstMateBooze()
    {
        if (firstMate != boozeGuy)
        {
            firstMate = boozeGuy;
            SetFirstMateButtons();
        }
        else
        {
            Debug.Log("This is already the first mate!");
        }
    }

    public void SetCoins()
    {
        coinCount++;
        coins.text = "Coins: " + coinCount.ToString();
    }

    private void SetButtonText()
    {
        coins.text = "Coins: " + coinCount.ToString();
        bulletsButton.text = "Buy\nBullets\n " + _bulletPrice + " coins\nOwned: " + bulletCount.ToString();
        cannonBallsButton.text = "Buy\nCannon Balls\n " + _cannonBallPrice + " coins\nOwned: " + cannonBallsCount.ToString();
        medicineButton.text = "Buy\nMedicine\n " + _medicinePrice + " coins\nOwned: " + medicineCount.ToString();

        if (crewHealth == maxHealth)
        {
            healButton.text = "Heal\nCrew\n[Healed]";
        }
        else if (crewHealth < maxHealth)
        {
            healButton.text = "Heal\nCrew\n " + _healPrice + " coins";
        }        
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
