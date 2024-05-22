using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class BoozeBehaviour : MonoBehaviour
{
    public bool _isCaught = false;
    public bool _isPaused = false;   

    public Camera _mainCamera;

    [SerializeField]
    private float _maxSpeed = 20f,
                  _minSpeed = 10f;

    private Vector3 _bottomLeftCorner,
                   _upperRightCorner;

    private Collider2D _collider;
    private bool _isCollidingWithMouth;    

    private Vector3 _velocity;

    void Start()
    {
        if (_mainCamera == null)
        {
            _mainCamera = Camera.main;
        }
        _collider = GetComponent<Collider2D>();
        GetEdgesOfTheScreen();//done
        GetRandomPosOnScreen();//done (needs checking)
        SetVelocity();//done (needs checking)
    }

    void Update()
    {
       if (!_isPaused) 
       {
            CheckIfStached();//Will find if the parrot has been dropped in the sack, Will check if the parrot is caought before the next update for it (since I want to let go of the mouse without it updateing _isCaught to false)
            CheckMouseCollision();
            if (!_isCaught) { ApplyVelocity(); }
       }
    }

    private void CheckIfStached()
    {
        if (_isCollidingWithMouth)
        {
            if(Input.GetMouseButtonUp(0) && _isCaught)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            _isCollidingWithMouth = false;
        }
    }

    private void CheckMouseCollision()
    {
        Vector2 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            if (_collider.OverlapPoint(mousePosition))
            {
                Debug.Log("Mouse is inside the object");
                _isCaught = true;
            }
        }
        else if (Input.GetMouseButton(0) && _isCaught)
        {
            transform.position = mousePosition;
        }
        else
        {
            _isCaught = false; 
        }
    }

    private void GoingOffScreen()
    {
        if (transform.position.x < _bottomLeftCorner.x - 1)
        {
            transform.position = new Vector3(_upperRightCorner.x + 1, transform.position.y, transform.position.z);
        }        
        if (transform.position.x > _upperRightCorner.x + 1)
        {
            transform.position = new Vector3(_bottomLeftCorner.x - 1, transform.position.y, transform.position.z);
        }
        
        if (transform.position.y < _bottomLeftCorner.y - 1)
        {
            transform.position = new Vector3(transform.position.x, _upperRightCorner.y + 1, transform.position.z);
        }
        
        if (transform.position.y > _upperRightCorner.y + 1)
        {
            transform.position = new Vector3(transform.position.x, _bottomLeftCorner.y - 1, transform.position.z);
        }
    }

    private void ApplyVelocity()
    {
        transform.position += _velocity * Time.deltaTime;
        GoingOffScreen();
        //RotateBottle(_velocity);
    }

    private void RotateBottle(Vector3 velocity)
    {
        SpriteRenderer sR = GetComponent<SpriteRenderer>();
        if (velocity.x < 0)
        {
            sR.flipX = true;
        }
        else
        {
            sR.flipX = false;
        }
        if (velocity.y < 0)
        {
            sR.flipY = true;
        }
        else
        {
            sR.flipY = false;

        }
    }

    private void SetVelocity()
    {
        Vector3 direction = GetRandomDirection();
        float speed = GetRandomSpeed();
        _velocity = direction * speed;
    }
    private Vector3 GetRandomDirection()
    {
        Vector3 direction;
        direction.x = UnityEngine.Random.Range(-1.0f, 1.0f);
        direction.y = UnityEngine.Random.Range(-1.0f, 1.0f); 
        direction.z = 0;
        return direction;
    }
    private float GetRandomSpeed()
    {
        float speed;
        speed = UnityEngine.Random.Range(_minSpeed, _maxSpeed);
        return speed;
    }

    private void GetRandomPosOnScreen()
    {
        Vector3 startingPos = Vector3.zero;
        startingPos.x = UnityEngine.Random.Range(_bottomLeftCorner.x, _upperRightCorner.x);
        startingPos.y = UnityEngine.Random.Range(_bottomLeftCorner.y, _upperRightCorner.y);

        transform.position = startingPos;
    }

    private void GetEdgesOfTheScreen()
    {
        _bottomLeftCorner = new Vector3(-9, -5, 0);
        _upperRightCorner = new Vector3(9, 5, 0);
        Debug.Log(_bottomLeftCorner + "," + _upperRightCorner);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Finish"))//collision.CompareTag("Finish"))
        {
            _isCollidingWithMouth = true;
            Debug.Log("Collided with " + collision.name);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Finish"))
        {
            _isCollidingWithMouth = false;
        }
    }
}
