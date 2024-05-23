using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BoozeMiniGame : MonoBehaviour
{
    public GameObject bottlePrefab, bombPrefab;
    private Vector3 _bottomLeftCorner, _topRightCorner;

    [SerializeField]
    private float _maxSpeed = 10f,
                  _minSpeed = 5f;

    private int _maxBottlesInScene = 10,
                _maxBombsInScene = 7,
                _bottlesInScene,
                _bombsInScene;
    public List<GameObject> bottleList, bombList;

    void Start()
    {
        bottleList = new List<GameObject>();
        bombList = new List<GameObject>();

        _bottomLeftCorner = new Vector3(-9, -5, 0);
        _topRightCorner = new Vector3(9, 5, 0);

        for (int i = 0; i < _maxBottlesInScene; i++)
        {
            SpawnBottles();
        }
        for (int i = 0; i < _maxBombsInScene; i++)
        {
            SpawnBombs();
        }
    }

    private void SpawnBombs()
    {
        Vector3 startingPos = Vector3.zero;
        startingPos.x = UnityEngine.Random.Range(_bottomLeftCorner.x, _topRightCorner.x);
        startingPos.y = UnityEngine.Random.Range(_bottomLeftCorner.y, _topRightCorner.y);
        var bomb = Instantiate(bombPrefab);
        _bombsInScene++;
        bombList.Add(bomb);
    }

    private void SpawnBottles()
    {
        Vector3 startingPos = Vector3.zero;
        startingPos.x = UnityEngine.Random.Range(_bottomLeftCorner.x, _topRightCorner.x);
        startingPos.y = UnityEngine.Random.Range(_bottomLeftCorner.y, _topRightCorner.y);
        var bottle = Instantiate(bottlePrefab);
        _bottlesInScene++;
        bottleList.Add(bottle);
    }

    void Update()
    {
        MoveObjects(bottleList);
        MoveObjects(bombList);
    }

    private void MoveObjects(List<GameObject> list)
    {
        foreach (GameObject obj in list)
        {
            obj.transform.position += GetVelocity() * Time.deltaTime; // the get velocity is the problem
        }
    }

    public Vector3 GetVelocity()
    {
        Vector3 direction = GetRandomDirection(); //THIS IS THE PROBLEM
        float speed = GetRandomSpeed(); //ALSO THIS
        var velocity = direction * speed;
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
}
