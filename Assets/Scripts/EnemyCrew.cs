using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCrew : MonoBehaviour
{
    public int _currentHealth, _maxHealth;
    public int _moveSpeed, _movement;
    public int _damage = 3;

    public Vector3 _targetPosition, _attackTarget;
    private Hex _targetHex, _previousHex;

    public Hex _currentHex;

    private void Awake()
    {
        _currentHealth = _maxHealth;
        _currentHex = transform.position.ToHex();
    }

    public IEnumerator MoveEnemy()
    {
        for (int remaining = _movement; remaining > 0; remaining--)
        {
            _currentHex = transform.position.ToHex();
            _targetHex = _targetPosition.ToHex();

            //if (isOnBad)
            //{
            //    isOnBad = false;
            //
            //    transform.position = _previousHex.ToWorld();
            //    _movement++;
            //
            //    GetComponent<Node>().ApplyTransform();
            //    StopCoroutine(MoveCharacter());
            //}

            var direction = _targetHex - _currentHex;

            if (direction.q == 0 && direction.r == 0)
            {
                remaining = 0;
                goto EndMovement;
            }
            if (direction.q != 0)
            {
                direction.q /= Mathf.Abs(direction.q);
            }
            if (direction.r != 0)
            {
                direction.r /= Mathf.Abs(direction.r);
            }

            _previousHex = _currentHex;
            _currentHex += direction;

            transform.position = _currentHex.ToWorld();
            _movement--;

        EndMovement:
            GetComponent<Node>().ApplyTransform();
            yield return new WaitForSeconds(0.25f);
        }
    }

    public bool TakeDamage(int damage)
    {
        _currentHealth -= damage;

        if (_currentHealth <= 0) return true;
        else return false;
    }
}