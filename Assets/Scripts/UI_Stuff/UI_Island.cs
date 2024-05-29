using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Island : MonoBehaviour
{
    public TextMeshProUGUI coins, bulletsButton, cannonBallsButton, monkeyButton, parrotButton, boozeButton, healButton;

    public TextMeshProUGUI continueButton, playerText;

    private int _bulletPrice = 2, _cannonBallPrice = 2, _healPrice = 2;
    private bool contButton = false;

    private GameManager gameManager;
    private Player selectedPlayer;

    private void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        selectedPlayer = gameManager.player1;

        SetButtonText();
        SetFirstMateButtons();
        SetCoins();
    }

    public void HealCrew()
    {
        if (CheckHealth(selectedPlayer.crew) && CanAfford(_healPrice))
        {
            foreach (Unit unit in selectedPlayer.crew)
            {
                if (unit.currentHealth < unit.maxHealth)
                {
                    unit.currentHealth = unit.maxHealth;
                }
            }

            selectedPlayer.coins -= _healPrice;
            SetCoins();
            healButton.text = "Heal\nCrew\n[Healed]";
        }
    }

    private void SetFirstMateButtons()
    {
        switch (selectedPlayer.firstMate)
        {
            case "Banana Joe":
                monkeyButton.text = "Banana\nJoe\n[Selected]";
                parrotButton.text = "Parrot\nmancer";
                boozeButton.text = "The \"Doctor\"";
                break;
            case "Parrotmancer":
                parrotButton.text = "Parrot\nmancer\n[Selected]";
                monkeyButton.text = "Banana\nJoe";
                boozeButton.text = "The \"Doctor\"";
                break;
            case "The Doctor":
                boozeButton.text = "The \"Doctor\"\n[Selected]";
                parrotButton.text = "Parrot\nmancer";
                monkeyButton.text = "Banana\nJoe";
                break;
            default:
                monkeyButton.text = "Banana\nJoe";
                parrotButton.text = "Parrot\nmancer";
                boozeButton.text = "The \"Doctor\"";
                break;
        }
    }

    public void SetFirstMateMonkey()
    {
        if (selectedPlayer.firstMate != "Banana Joe")
        {
            selectedPlayer.firstMate = "Banana Joe";
            SetFirstMateButtons();
        }
    }

    public void SetFirstMateParrot()
    {
        if (selectedPlayer.firstMate != "Parrotmancer")
        {
            selectedPlayer.firstMate = "Parrotmancer";
            SetFirstMateButtons();
        }
    }

    public void SetFirstMateBooze()
    {
        if (selectedPlayer.firstMate != "The Doctor")
        {
            selectedPlayer.firstMate = "The Doctor";
            SetFirstMateButtons();
        }
    }

    public void SetCoins()
    {
        coins.text = "Coins: " + selectedPlayer.coins.ToString();
    }

    private void SetButtonText()
    {
        if (CheckHealth(selectedPlayer.crew))
        {
            healButton.text = "Heal\nCrew\n[Healed]";
        }
        else
        {
            healButton.text = "Heal\nCrew\n " + _healPrice + " coins";
        }

        SetCoins();
        bulletsButton.text = "Bullets\n " + _bulletPrice + " coins\nOwned: " + selectedPlayer.ammo.ToString();
        cannonBallsButton.text = "Cannon Balls\n " + _cannonBallPrice + " coins\nOwned: " + selectedPlayer.cannonballs.ToString(); 
    }

    public void BuyBullets()
    {
        if (CanAfford(_bulletPrice))
        {
            selectedPlayer.ammo++;
            bulletsButton.text = "Bullets\n " + _bulletPrice + " coins\nOwned: " + selectedPlayer.ammo.ToString();

            selectedPlayer.coins -= _cannonBallPrice;
            SetCoins();
        }
    }

    public void BuyCannonBalls()
    {
        if (CanAfford(_cannonBallPrice))
        {

            selectedPlayer.cannonballs++;
            cannonBallsButton.text = "Cannon Balls\n " + _cannonBallPrice + " coins\nOwned: " + selectedPlayer.cannonballs.ToString();

            selectedPlayer.coins -= _cannonBallPrice;
            SetCoins();
        }
    }

    public void Continue()
    {
        if (contButton == false)
        {
            contButton = true;
            selectedPlayer = gameManager.player2;

            SetButtonText();
            SetFirstMateButtons();
            SetCoins();

            continueButton.text = "Leave\nIsland";
            playerText.text = "Player 2";

            gameManager.player1 = selectedPlayer;
        }
        else
        {
            gameManager.player2 = selectedPlayer;
            gameManager.NewLevel();
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

    private bool CanAfford(int cost)
    {
        return (selectedPlayer.coins >= cost);
    }
}
