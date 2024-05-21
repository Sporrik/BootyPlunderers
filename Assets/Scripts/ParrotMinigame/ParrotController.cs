using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class ParrotController : MonoBehaviour
{
    [SerializeField]
    GameObject Parrot;

    [SerializeField]
    private int _amountOfParrots = 4,
                _amountCapturedToWin = 10;
    public int _amountCaptured = 0;
    private GameObject [] _parrots;

    private float _timeSet = 10f,
                  _timeleft;
                


    // Start is called before the first frame update
    void Start()
    {
        SetTime();
        SpawnBirds();
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
        disableParrots();
    }

    private void disableParrots()
    {
        for (int i = 0; i < _amountOfParrots; i++)
        {
            if (_parrots[i] != null)
            {
                //_parrots[i]._isPaused;
            }
        }
    }

    private void SetTime()
    {
        _timeleft = _timeSet;
    }
    private void SpawnBirds()
    {
        _parrots = new GameObject[_amountOfParrots];
        for (int i = 0; i < _amountOfParrots; i++)
        {
            GameObject parrot = Instantiate(Parrot);
            _parrots[i] = parrot;
        }
    }
    private void UpdateScoring()
    {
        for  (int i = 0; i < _amountOfParrots; i++)
        {
            if (_parrots[i] == null) { _amountCaptured++; }
        }
    }
}
