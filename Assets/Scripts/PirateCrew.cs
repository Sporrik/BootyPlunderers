using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PirateCrew : MonoBehaviour
{
    [SerializeField]
    private int _maxHealth, _currentHealth;
    private int _movement;
    private int _moveSpeed = 3;

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

            _currentHex += direction;

            transform.position = _currentHex.ToWorld();
            _movement--;

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
}
