using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
{
    public Slider moveSlider;
    public Slider healthSlider;

    public TextMeshProUGUI coins;
    public int coinCount;

    public void SetHUD(Unit unit)
    {
        moveSlider.maxValue = unit.moveSpeed;
        moveSlider.value = unit.movement;

        healthSlider.maxValue = unit.maxHealth;
        healthSlider.value = unit.currentHealth;
    }

    public void SetHP(int hp)
    {
        healthSlider.value = hp;
    }

    public void SetMove(int move)
    {
        moveSlider.value = move;
    }

    public void SetCoins()
    {  
        coins.text = "Coins: " + coinCount.ToString();
    }    
}
