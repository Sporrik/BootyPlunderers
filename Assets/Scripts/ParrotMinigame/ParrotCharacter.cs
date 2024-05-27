using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ParrotCharacter : MonoBehaviour
{
    public bool _isPaused  = false; 
    [SerializeField]
    private float _jumpHeight = 100.0f;// The height that the player should reach
    public bool _gameEnded = false;

    [SerializeField]
    private float _fallGravityScale = 0.5f;

    private Vector3 _bottomLeft, _topRight;

    private Vector2 _startingPoint;

    private Rigidbody2D _rb;

    // Start is called before the first frame update
    void Start()
    {
        _bottomLeft = Camera.main.ViewportToScreenPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        _topRight = Camera.main.ViewportToScreenPoint(new Vector3(1, 1, Camera.main.nearClipPlane));
        _startingPoint = Camera.main.ScreenToWorldPoint(new Vector2(_topRight.x / 6, _topRight.y / 2));
        transform.position = _startingPoint;
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = _fallGravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_gameEnded || !_isPaused)
        {
            CheckInput();
        }
        LimitCharacterToScreen();//Maybe change it to finish game if player touches the bottom 
    }


    private void LimitCharacterToScreen()
    {
        Vector3 _bottomLeftworld = Camera.main.ScreenToWorldPoint(_bottomLeft);
        Vector3 _topRightworld = Camera.main.ScreenToWorldPoint(_topRight);
        Vector3 position= transform.position;
        position.y = Mathf.Clamp(position.y, _bottomLeftworld.y, _topRightworld.y);
        
        transform.position = position;

        //Vector3 bottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        //Vector3 topRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.nearClipPlane));
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
        transform.rotation = Quaternion.Euler(0, 0, -90);
    }
}
