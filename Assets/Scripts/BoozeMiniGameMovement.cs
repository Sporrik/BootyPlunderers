using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoozeMiniGameMovement : MonoBehaviour
{
    private float _speed, _minSpeed = 10, _maxSpeed = 15;
    private Vector3 _direction, _velocity;
    public bool isMoving;
    private Vector3 _bottomLeftCorner, _topRightCorner;

    private void Start()
    {
        //Destroy(gameObject);
    }
    private void OnEnable()
    {
        _bottomLeftCorner = new Vector3(-54, -4, -50);
        _topRightCorner = new Vector3(-36, 7, -50);
        isMoving = true;
        _velocity = GetVelocity();        
    }

    private void Update()
    {
        if (isMoving)
        {
            transform.position += _velocity * Time.deltaTime;
            GoingOffScreen();
        }
    }

    public Vector3 GetVelocity()
    {
        _direction = GetRandomDirection();
        _speed = GetRandomSpeed();
        var velocity = _direction * _speed;
        return velocity;
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

    private void GoingOffScreen()
    {
        if (transform.position.x < _bottomLeftCorner.x - 1)
        {
            transform.position = new Vector3(_topRightCorner.x + 1, transform.position.y, transform.position.z);
        }
        if (transform.position.x > _topRightCorner.x + 1)
        {
            transform.position = new Vector3(_bottomLeftCorner.x - 1, transform.position.y, transform.position.z);
        }
        if (transform.position.y < _bottomLeftCorner.y - 1)
        {
            transform.position = new Vector3(transform.position.x, _topRightCorner.y + 1, transform.position.z);
        }
        if (transform.position.y > _topRightCorner.y + 1)
        {
            transform.position = new Vector3(transform.position.x, _bottomLeftCorner.y - 1, transform.position.z);
        }
    }
}
