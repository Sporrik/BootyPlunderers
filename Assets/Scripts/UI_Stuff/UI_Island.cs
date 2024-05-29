using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum IslandState { P1_TURN, P2_TURN }

public class UI_Island : MonoBehaviour
{
    public int coinCount;
    public TextMeshProUGUI coins, bulletsButton, cannonBallsButton, monkeyButton, parrotButton, boozeButton, healButton;

    public TextMeshProUGUI continueButton;

    public GameObject monkeyGuy, parrotGuy, boozeGuy, firstMate;

    private int _bulletPrice = 2, _cannonBallPrice = 2, _healPrice = 2;

    private IslandState state;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();

        state = IslandState.P1_TURN;

        firstMate = monkeyGuy; //change later
        SetButtonText();
        SetFirstMateButtons();
        SetCoins();
    }

    public void HealCrew()
    {
        switch (state)
        {
            case IslandState.P1_TURN:
                if (CheckHealth(gameManager.player1.crew) && CanAfford(_healPrice))
                {
                    HealCrew(gameManager.player1.crew);
                }

                break;
            case IslandState.P2_TURN:
                if (CheckHealth(gameManager.player2.crew))
                {
                    HealCrew(gameManager.player2.crew);
                }
                break;
        }
    }

    private void SetFirstMateButtons()
    {
        if (firstMate == monkeyGuy)
        {
            monkeyButton.text = "Banana\nJoe\n[Selected]";
            parrotButton.text = "Parrotmancer";
            boozeButton.text = "The \"Doctor\"";
        }
        else if (firstMate == parrotGuy)
        {
            parrotButton.text = "Parrotmancer\n[Selected]";
            monkeyButton.text = "Banana\nJoe";
            boozeButton.text = "The \"Doctor\"";
        }
        else if (firstMate == boozeGuy)
        {
            boozeButton.text = "The \"Doctor\"\n[Selected]";
            parrotButton.text = "Parrotmancer";
            monkeyButton.text = "Banana\nJoe";
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
        switch (state)
        {
            case IslandState.P1_TURN:
                coinCount = gameManager.player1.coins;
                break;
            case IslandState.P2_TURN:
                coinCount = gameManager.player2.coins;
                break;
        }
        coins.text = "Coins: " + coinCount.ToString();
    }

    private void SetButtonText()
    {
        switch (state)
        {
            case IslandState.P1_TURN:
                if (CheckHealth(gameManager.player1.crew))
                {
                    healButton.text = "Heal\nCrew\n[Healed]";
                }
                else
                {
                    healButton.text = "Heal\nCrew\n " + _healPrice + " coins";
                }
                
                SetCoins();
                bulletsButton.text = "Bullets\n " + _bulletPrice + " coins\nOwned: " + gameManager.player1.ammo.ToString();
                cannonBallsButton.text = "Cannon Balls\n " + _cannonBallPrice + " coins\nOwned: " + gameManager.player1.cannonballs.ToString();

                break;
            case IslandState.P2_TURN:
                if (CheckHealth(gameManager.player2.crew))
                {
                    healButton.text = "Heal\nCrew\n[Healed]";
                }
                else
                {
                    healButton.text = "Heal\nCrew\n " + _healPrice + " coins";
                }

                SetCoins();
                bulletsButton.text = "Bullets\n " + _bulletPrice + " coins\nOwned: " + gameManager.player2.ammo.ToString();
                cannonBallsButton.text = "Cannon Balls\n " + _cannonBallPrice + " coins\nOwned: " + gameManager.player2.cannonballs.ToString();

                break;
        }     
    }

    public void BuyBullets()
    {
        if (CanAfford(_bulletPrice))
        {
            switch (state)
            {
                case IslandState.P1_TURN:
                    gameManager.player1.ammo++;
                    cannonBallsButton.text = "Cannon Balls\n " + _cannonBallPrice + " coins\nOwned: " + gameManager.player1.ammo.ToString();
                    break;
                case IslandState.P2_TURN:
                    gameManager.player2.ammo++;
                    cannonBallsButton.text = "Cannon Balls\n " + _cannonBallPrice + " coins\nOwned: " + gameManager.player2.ammo.ToString();
                    break;
            }

            coinCount -= _cannonBallPrice;
            SetCoins();
        }
    }

    public void BuyCannonBalls()
    {
        if (CanAfford(_cannonBallPrice))
        {
            switch (state)
            {
                case IslandState.P1_TURN:
                    gameManager.player1.cannonballs++;
                    cannonBallsButton.text = "Cannon Balls\n " + _cannonBallPrice + " coins\nOwned: " + gameManager.player1.cannonballs.ToString();
                    break;
                case IslandState.P2_TURN:
                    gameManager.player2.cannonballs++;
                    cannonBallsButton.text = "Cannon Balls\n " + _cannonBallPrice + " coins\nOwned: " + gameManager.player2.cannonballs.ToString();
                    break;
            }

            coinCount -= _cannonBallPrice;
            SetCoins();
        }
    }

    private bool CheckHealth(Unit[] crewArray)
    {
        foreach (Unit unit in crewArray)
        {
            if (unit.currentHealth < unit.maxHealth)
            {
                return true;
            }
        }

        return false;
    }

    private void HealCrew(Unit[] crewArray)
    {
        foreach (Unit unit in crewArray)
        {
            if (unit.currentHealth < unit.maxHealth)
            {
                unit.currentHealth = unit.maxHealth;
            }  
        }

        coinCount -= _healPrice;
        healButton.text = "Heal\nCrew\n[Healed]";
    }

    private bool CanAfford(int cost)
    {
        return (coinCount <= cost);
    }
}
