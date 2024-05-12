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
    public int collectedTreasure;

    public GameObject heldObject;

    public bool isOnBad = false;
    public bool hasAttacked = false;

    public Vector3 _targetPosition, _attackTarget;
    private Hex _targetHex, _previousHex;
    public Hex _currentHex, _attackHex;

    private void Awake()
    {
        _maxHealth = 10;
        _currentHealth = _maxHealth;
        _movement = _moveSpeed;
        _currentHex = transform.position.ToHex();
    }

    public IEnumerator MoveCharacter()
    {
        _targetHex = _targetPosition.ToHex();

        for (int remaining = _movement; remaining > 0; remaining--)
        {
            _currentHex = transform.position.ToHex();

            if (isOnBad)
            {
                isOnBad = false;

                _targetHex = _previousHex;
                _currentHex = _previousHex;
                transform.position = _previousHex.ToWorld();
                _movement++;

                GetComponent<Node>().ApplyTransform();
                StopCoroutine(MoveCharacter());
            }
            
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" || collision.tag == "Border")
        {
            isOnBad = true;

            Debug.Log("Player collides with enemy!");
        }

        if (collision.tag == "Treasure")
        {
            if (heldObject == null)
            {
                Debug.Log("Player collides with treasure!");
                var treasure = collision.gameObject;
                treasure.transform.localScale = new Vector2(0.5f, 0.5f);
                treasure.transform.SetParent(transform);
                treasure.transform.position += Vector3.up * 0.5f;
                heldObject = treasure;
            }            
        }

        if (collision.tag == "ClaimPoint")
        {
            if (heldObject != null)
            {
                ++collectedTreasure;
                Debug.Log("Player collides with Claim Point!");
                var claimPoint = collision.gameObject;
                Destroy(heldObject);
                heldObject = null;
            }
        }
    }

    public bool TakeDamage(int damage)
    {
        _currentHealth -= damage;

        if (_currentHealth <= 0) return true;
        else return false;
    }
}
