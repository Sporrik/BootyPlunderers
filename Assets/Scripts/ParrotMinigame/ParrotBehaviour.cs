using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class ParrotBehaviour : MonoBehaviour
{
    public bool _isCaught = false;
    public bool _isPaused = false;
    

    [SerializeField]
    Camera _mainCamera;

    [SerializeField]
    private float _maxSpeed = 20f,
                  _minSpeed = 10f;

    private Vector3 _bottomLeftCorner,
                   _UpperRightCorner;

    private Collider2D _collider;
    private bool _isCollidingWithSack;
    

    private Vector3 _velocity;

    // Start is called before the first frame update
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

    // Update is called once per frame
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
        if (_isCollidingWithSack)
        {
            if(Input.GetMouseButtonUp(0) && _isCaught)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            _isCollidingWithSack = false;
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
        SpriteRenderer sR = GetComponent<SpriteRenderer>();
        if (transform.position.x < _bottomLeftCorner.x - sR.size.x * 10)
        {
            transform.position = new Vector3(_UpperRightCorner.x, transform.position.y, transform.position.z);
        }
        if (transform.position.x > _UpperRightCorner.x + sR.size.x * 10)
        {
            transform.position = new Vector3(_bottomLeftCorner.x, transform.position.y, transform.position.z);
        }
        if (transform.position.y < _bottomLeftCorner.y - sR.size.y * 10)
        {
            transform.position = new Vector3(transform.position.x, _UpperRightCorner.y, transform.position.z);
        }
        if (transform.position.y > _UpperRightCorner.y + sR.size.y * 10)
        {
            transform.position = new Vector3(transform.position.x, _bottomLeftCorner.y, transform.position.z);
        }

    }
    private void ApplyVelocity()
    {
        transform.position += _velocity * Time.deltaTime;
        GoingOffScreen();
        RotateParrot(_velocity);
    }

    private void RotateParrot(Vector3 velocity)
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
        direction.x = UnityEngine.Random.Range(-1.0f, 1.0f); ;
        direction.y = UnityEngine.Random.Range(-1.0f, 1.0f); ;
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
        Vector3 viewportPos = Vector3.zero;
        viewportPos.x = UnityEngine.Random.Range(0.0f, 1.0f);
        viewportPos.y = UnityEngine.Random.Range(0.0f, 1.0f);

        transform.position = _mainCamera.ViewportToScreenPoint(viewportPos);
    }
    private void GetEdgesOfTheScreen()
    {
        _bottomLeftCorner = new Vector3(0, 0, _mainCamera.nearClipPlane);
        _UpperRightCorner = new Vector3(1, 1, _mainCamera.nearClipPlane);
        Debug.Log(_bottomLeftCorner + "," + _UpperRightCorner);

        _bottomLeftCorner = _mainCamera.ViewportToScreenPoint(_bottomLeftCorner);
        _UpperRightCorner = _mainCamera.ViewportToScreenPoint(_UpperRightCorner);
        Debug.Log(_bottomLeftCorner + "," + _UpperRightCorner);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Finish"))//collision.CompareTag("Finish"))
        {
            _isCollidingWithSack = true;
            Debug.Log("Collided with " + collision.name);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Finish"))
        {
            _isCollidingWithSack = false;
        }
    }



}
