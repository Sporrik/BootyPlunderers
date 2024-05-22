using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UIElements;

public class MonkeyMiniGame : MonoBehaviour
{
    private GameObject monkey;
    public Collider2D monkeyCollider;
    public GameObject coinPrefab;

    private int _maxCoins = 10,
                _coinSpeed = 3,
                _coinsInScene,
                _monkeySpeed = 7,
                _collectedCoins;

    private float _timer = 12;

    private List<GameObject> _coins;

    public TextMeshProUGUI coinsCollectedText, timerText, gameOverText;
    private bool _isCounting = true;

    void Start()
    {
        _coins = new List<GameObject>();
        monkey = monkeyCollider.gameObject;
        gameOverText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (_isCounting)
        {
            _timer -= Time.deltaTime;
            timerText.text = Mathf.Ceil(_timer).ToString();
        }

        if (_timer > 0)
        {
            monkey.transform.position += new Vector3(Input.GetAxis("Horizontal") * Time.deltaTime * _monkeySpeed, 0, 0);
            if (monkey.transform.position.x < -8)
            {
                monkey.transform.position = new Vector3(-8, monkey.transform.position.y, monkey.transform.position.z);
            }
            if (monkey.transform.position.x > 8)
            {
                monkey.transform.position = new Vector3(8, monkey.transform.position.y, monkey.transform.position.z);
            }

            if (_coinsInScene <= _maxCoins)
            {
                for (int i = 0; i <= _maxCoins; i++)
                {
                    SpawnCoin();
                }
            }

            CheckCollisions();

            foreach (GameObject coin in _coins)
            {
                coin.transform.position -= new Vector3(0, 1, 0) * Time.deltaTime * _coinSpeed;

                if (coin.transform.position.y < -6)
                {
                    _coins.Remove(coin);
                    _coinsInScene--;
                    Destroy(coin);
                    break;
                }
            }            
        }

        else if (_timer <= 0)
        {
            _isCounting = false;
            if (_coins.Count > 0)
            {
                foreach (GameObject coin in _coins)
                {
                    Destroy(coin);
                }
            }            
            gameOverText.gameObject.SetActive(true);
        }
    }

    private void CheckCollisions()
    {
        foreach (GameObject coin in _coins)
        {
            if (monkeyCollider.IsTouching(coin.GetComponent<Collider2D>()))
            {
                _coinsInScene--;
                _coins.Remove(coin);
                Destroy(coin);
                _collectedCoins++;
                coinsCollectedText.text = _collectedCoins.ToString();
                break;
            }
        }        
    }

    private void SpawnCoin()
    {
        Vector3 position = new Vector3(UnityEngine.Random.Range(-8, 8), UnityEngine.Random.Range(6, 18), -2);
        var coin = Instantiate(coinPrefab);
        coin.transform.position = position;
        _coinsInScene++;
        _coins.Add(coin);
    }
}
