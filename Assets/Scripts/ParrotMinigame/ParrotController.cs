using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParrotController : MonoBehaviour
{
    [SerializeField]
    GameObject Parrot;

    [SerializeField]
    private int _amountOfParrots = 4,
                _amountCapturedToWin = 10;
    private int _amountCaptured = 0;
    private List<GameObject> _parrots;


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < _amountOfParrots; i++)
        {
            GameObject parrot = Instantiate(Parrot);
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateScoring();
    }

    private void UpdateScoring()
    {
        _amountCaptured = _amountOfParrots - _parrots.Count;
    }
}
