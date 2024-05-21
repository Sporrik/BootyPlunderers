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

    public Image healthSliderFill;
    public Color green, yellow, red;
    

    public void SetHUD(Unit unit)
    {
        moveSlider.maxValue = unit.moveSpeed;
        moveSlider.value = unit.movement;

        healthSlider.maxValue = unit.maxHealth;
        healthSlider.value = unit.currentHealth;

        green = new Color(0.3254902f, 0.7647059f, 0.2784314f);
        yellow = new Color(0.8641509f, 0.8536432f, 0.3093661f);
        red = new Color(0.8627451f, 0.3098039f, 0.3624313f);
    }

    public void SetHP(int hp)
    {
        healthSlider.value = hp;
        if (hp >= healthSlider.maxValue/2)
        {
            healthSliderFill.color = green;
        }
        else if (hp < healthSlider.maxValue / 2 && hp > healthSlider.maxValue / 4  )
        {
            healthSliderFill.color = yellow;
        }
        else if (hp <= healthSlider.maxValue / 4)
        {
            healthSliderFill.color = red;
        }
    }

    public void SetMove(int move)
    {
        moveSlider.value = move;
    }

    public void SetCoins()
    {  
        coins.text = "Coins: " + coinCount.ToString();
    }

    private void UpdateSliderColor(int health)
    {
        healthSlider.value = health;
        
    }
}
