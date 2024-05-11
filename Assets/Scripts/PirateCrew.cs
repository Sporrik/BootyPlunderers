using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PirateCrew : MonoBehaviour
{
    public int _maxHealth, _currentHealth;
    public int _movement;
    public int _moveSpeed;
    public int _damage = 3;

    private bool _isCollidingEnemy = false;
    public GameObject HeldObject;

    public Vector3 _targetPosition;
    private Hex _currentHex, _targetHex;    

    private void Awake()
    {
        _maxHealth = 10;
        _currentHealth = _maxHealth;
        _movement = _moveSpeed;
    }

    public IEnumerator MoveCharacter()
    {
        for (int remaining = _movement; remaining > 0; remaining--)
        {
            _currentHex = transform.position.ToHex();
            _targetHex = _targetPosition.ToHex();

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

            Hex previouseHex = _currentHex;
            _currentHex += direction;
            
            if (!_isCollidingEnemy)
            {
                transform.position = _currentHex.ToWorld();
                _movement--;
            }
            else
            {
                _currentHex = previouseHex;
                transform.position = _currentHex.ToWorld();
            }

            EndMovement:
            GetComponent<Node>().ApplyTransform();
            yield return new WaitForSeconds(0.25f);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            _isCollidingEnemy = true;
            Debug.Log("Player collides with enemy!");
        }

        if (collision.tag == "Treasure")
        {
            if (HeldObject == null)
            {
                Debug.Log("Player collides with treasure!");
                var treasure = collision.gameObject;
                treasure.transform.localScale = new Vector2(0.5f, 0.5f);
                treasure.transform.SetParent(transform);
                treasure.transform.position += Vector3.up * 0.5f;
                HeldObject = treasure;
            }            
        }

        if (collision.tag == "ClaimPoint")
        {
            if (HeldObject != null)
            {
                Debug.Log("Player collides with Claim Point!");
                var claimPoint = collision.gameObject;
                Destroy(HeldObject);
                HeldObject = null;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _isCollidingEnemy = false; //Assuming there is only one collision with the enemies
    }

    public bool TakeDamage(int damage)
    {
        _currentHealth -= damage;

        if (_currentHealth <= 0) return true;
        else return false;
    }
}
