using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class BottleController : MonoBehaviour
{
    [SerializeField]
    GameObject Bottle,
               Bomb;

    [SerializeField]
    private int _amountOfBottles = 10,
                _amountOfBombs = 7,
                _amountCapturedToWin = 10;
    public int _score = 0;
    private GameObject [] _bottles;
    private GameObject[] _bombs; 

    private float _timeSet = 10f,
                  _timeleft;
                


    // Start is called before the first frame update
    void Start()
    {
        SetTime();
        SpawnObjects();
    }

    private void UpdateTime()
    {
        _timeleft -= Time.deltaTime;
    }



    // Update is called once per frame
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
            GameObject bottle = Instantiate(Bottle);
            _bottles[i] = bottle;
        }
        _bombs = new GameObject[_amountOfBombs];
        for (int i = 0; i < _amountOfBombs; i++)
        {
            GameObject bomb = Instantiate(Bomb);
            _bombs[i] = bomb;
        }
    }
    private void UpdateScoring()
    {
        for (int i = 0; i < _amountOfBottles; i++)
        {
            if (_bottles[i] == null) { _score++; }
        }
        for (int i = 0; i < _amountOfBombs; i++)
        {
            if (_bombs[i] == null) { _score--; }
        }
    }
}
