using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;

public class BottleController : MonoBehaviour
{
    [SerializeField]
    GameObject bottlePrefab,
               bombPrefab;

    [SerializeField]
    private int _amountOfBottles = 10,
                _amountOfBombs = 7,
                _amountCapturedToWin = 10;
    public int score = 0;
    private GameObject [] _bottles;
    private GameObject[] _bombs;

    private float _timeSet = 10f,
                  _timeleft;

    void Start()
    {
        SetTime();
        SpawnObjects();
    }

    private void UpdateTime()
    {
        _timeleft -= Time.deltaTime;
    }

    void Update()
    {
        UpdateTime();
        if (_timeleft < 0)
        {
            OnTimerEnd();
        }
    }

    private void OnTimerEnd()
    {
        UpdateScoring();
    }    

    private void SetTime()
    {
        _timeleft = _timeSet;
    }

    private void SpawnObjects()
    {
        _bottles = new GameObject[_amountOfBottles];
        for (int i = 0; i < _amountOfBottles; i++)
        {
            GameObject bottle = Instantiate(bottlePrefab);
            _bottles[i] = bottle;
        }
        _bombs = new GameObject[_amountOfBombs];
        for (int i = 0; i < _amountOfBombs; i++)
        {
            GameObject bomb = Instantiate(bombPrefab);
            _bombs[i] = bomb;
        }
    }

    private void UpdateScoring()
    {
        for (int i = 0; i < _amountOfBottles; i++)
        {
            if (_bottles[i] == null) { score++; }
        }
        for (int i = 0; i < _amountOfBombs; i++)
        {
            if (_bombs[i] == null) { score--; }
        }
    }
}
