using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PirateCrew : MonoBehaviour
{
    [SerializeField]
    public int _maxHealth, _currentHealth;
    public int _movement;
    private int _moveSpeed = 3;

    private bool _isCollidingEnemy = false;

    private Vector3 _targetPosition;
    private Hex _currentHex, _targetHex;

    private void Awake()
    {
        _currentHealth = _maxHealth;
        _movement = _moveSpeed;
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse1) && _movement > 0)
        {
            _targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            StartCoroutine(MoveCharacter());
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(NextTurn());
        }
    }

    IEnumerator MoveCharacter()
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

    IEnumerator NextTurn()
    {
        _movement = _moveSpeed;
        yield return new WaitForSeconds(0.25f);
    }

    private void OnTriggerEnter(Collider collideObject)
    {
        if (collideObject.tag == "Enemy")
        {
            _isCollidingEnemy = true;
            Debug.Log("Player collides with enemy!");
        }
    }
    private void OnTriggerExit(Collider collideObject) 
    {
        _isCollidingEnemy = false; //Assuming there is only one collision with the enemies
        
    }
}
