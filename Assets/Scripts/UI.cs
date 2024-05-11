using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
{
    public Slider moveSlider;
    public Slider healthSlider;

    public void SetHUD(PirateCrew pirate)
    {
        moveSlider.maxValue = pirate._moveSpeed;
        moveSlider.value = pirate._movement;

        healthSlider.maxValue = pirate._maxHealth;
        healthSlider.value = pirate._currentHealth;
    }

    public void SetHUD(EnemyCrew enemy)
    {
        healthSlider.maxValue = enemy._maxHealth;
        healthSlider.value = enemy._currentHealth;
    }

    public void SetHP(int hp)
    {
        healthSlider.value = hp;
    }

    public void SetMove(int move)
    {
        moveSlider.value = move;
    }
}
