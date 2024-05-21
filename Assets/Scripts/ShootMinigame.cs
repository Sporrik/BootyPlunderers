using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class ShootMinigame : MonoBehaviour
{
    public Collider2D miss, miss2, hit, hit2, bullseye, pointer;

    public int enemyHealth, hitDamage, bullseyeDamage, shotCount, maxShots;
    private int _speed = 1;

    public Slider enemyHealthSlider;
    public Image sliderFill;
    public UnityEngine.Color green, yellow, red;

    private void Start()
    {        
        enemyHealth = 100;
        enemyHealthSlider.maxValue = 100;
        enemyHealthSlider.value = enemyHealth;
        maxShots = 1; //would be the amount of bullets?
        hitDamage = 10;
        bullseyeDamage = 15;

        this.enabled = false;
    }

    void Update()
    {
        if (shotCount < maxShots)
        {
            pointer.gameObject.transform.position = new Vector3(Mathf.Sin(Time.time * _speed) * 5, 4f, -3f); //*5 for distance

            if (Input.GetKeyDown(KeyCode.Space))
            {
                CheckCollision();
                shotCount++;
            }
        }               
    }

    private void UpdateSlider(int health)
    {
        enemyHealthSlider.value = health;
        if (health > 50)
        {
            sliderFill.color = green;
        }
        else if (health <= 50 && health > 26)
        {
            sliderFill.color = yellow;
        }
        else if (health <= 25)
        {
            sliderFill.color = red;
        }
    }

    private void CheckCollision()
    {
        if (pointer.IsTouching(miss) || pointer.IsTouching(miss2))
        {            
            Debug.Log($"Miss! Enemy health: {enemyHealth}");
            UpdateSlider(enemyHealth);
        }
        else if (pointer.IsTouching(hit) || pointer.IsTouching(hit2))
        {
            enemyHealth -= hitDamage;
            Debug.Log($"Hit! Enemy health: {enemyHealth}");
            UpdateSlider(enemyHealth);
        }
        else if (pointer.IsTouching(bullseye))
        {
            enemyHealth -= bullseyeDamage;
            Debug.Log($"Bullseye! Enemy health: {enemyHealth}");
            UpdateSlider(enemyHealth);
        }
    }

    IEnumerator ResolveAttack()
    {
        yield return new WaitForSeconds(2f);
    }
}
