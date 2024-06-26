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
    public bool _gameEnded = false,
                _gameWon = false;

    [SerializeField]
    private float _fallGravityScale = 0.5f;

    private Vector3 _bottomLeft, _topRight;

    private Vector2 _startingPoint;

    private Rigidbody2D _rb;

    public Camera parrotCam;

    // Start is called before the first frame update
    void Start()
    {
        _bottomLeft = parrotCam.ViewportToScreenPoint(new Vector3(0, 0, parrotCam.nearClipPlane));
        _topRight = parrotCam.ViewportToScreenPoint(new Vector3(1, 1, parrotCam.nearClipPlane));
        _startingPoint = parrotCam.ScreenToWorldPoint(new Vector2(_topRight.x / 6, _topRight.y / 2));
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
        Vector3 _bottomLeftworld = parrotCam.ScreenToWorldPoint(_bottomLeft);
        Vector3 _topRightworld = parrotCam.ScreenToWorldPoint(_topRight);
        Vector3 position= transform.position;
        position.y = Mathf.Clamp(position.y, _bottomLeftworld.y, _topRightworld.y);
        
        transform.position = position;

        //Vector3 bottomLeft = parrotCam.ScreenToWorldPoint(new Vector3(0, 0, parrotCam.nearClipPlane));
        //Vector3 topRight = parrotCam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, parrotCam.nearClipPlane));
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
        if (!_gameWon)
        {
            transform.rotation = Quaternion.Euler(0, 0, -90);
        }
    }
}
