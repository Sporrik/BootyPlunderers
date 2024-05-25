using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ParrotCharacter : MonoBehaviour
{
    [SerializeField]
    private float _jumpHeight = 100.0f;// The height that the player should reach
    private bool _gameEnded = false;

   

    [SerializeField]
    Vector2 _startingPoint = new Vector2 (100,Screen.height/2);

    private Rigidbody2D _rb;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = _startingPoint;
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_gameEnded)
        {
            CheckInput();
        }
            LimitCharacterToScreen();//Maybe change it to finish game if player touches the bottom 
    }


    private void LimitCharacterToScreen()
    {
        transform.position = new Vector2 (transform.position.x,Mathf.Clamp(transform.position.y,0 ,Screen.height));
    }

    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _rb.velocity = new Vector2(0, _jumpHeight);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Finish"))
        {
            FinishGame();
        }
    }

    private void FinishGame()
    {
        _gameEnded = true;
    }
}
