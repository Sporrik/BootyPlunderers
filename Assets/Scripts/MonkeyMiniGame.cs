using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MonkeyMiniGame : MonoBehaviour
{
    public GameObject monkey;
    private int _monkeySpeed = 7;
    public GameObject coinPrefab;
    private int _maxCoins = 10,
                _coinSpeed = 3;
    private int _coinsInScene;
    private List<GameObject> _coins;

    void Start()
    {
        _coins = new List<GameObject>();
    }

    void Update()
    {
        monkey.transform.position += new Vector3 (Input.GetAxis("Horizontal") * Time.deltaTime * _monkeySpeed, 0, 0);
        if (monkey.transform.position.x < -8)
        {
            monkey.transform.position = new Vector3( -8, monkey.transform.position.y, monkey.transform.position.z);
        }
        if (monkey.transform.position.x > 8)
        {
            monkey.transform.position = new Vector3( 8, monkey.transform.position.y, monkey.transform.position.z);
        }

        if (_coinsInScene <= _maxCoins)
        {
            for (int i = 0; i <= _maxCoins; i++)
            {
                SpawnCoin();
            }
        }

        foreach (GameObject coin in _coins)
        {            
            coin.transform.position -= new Vector3(0, 1, 0) * Time.deltaTime * _coinSpeed;
            if (coin.transform.position.y < -6)
            {
                _coins.Remove(coin);
                _coinsInScene--;
                Destroy(coin);                
            }
        }        
    }

    private void SpawnCoin()
    {
        Vector3 position = new Vector3(UnityEngine.Random.Range(-8, 8), UnityEngine.Random.Range(6, 10), -2);
        var coin = Instantiate(coinPrefab);
        coin.transform.position = position;
        _coinsInScene++;
        _coins.Add(coin);        
    }
}
