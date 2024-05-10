using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCrew : MonoBehaviour
{
    public int _currentHealth, _maxHealth;
    public int _moveSpeed, _movement;
    public int _damage = 3;

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    public bool TakeDamage(int damage)
    {
        _currentHealth -= damage;

        if (_currentHealth <= 0) return true;
        else return false;
    }
}