using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.SocialPlatforms.Impl;

public class ParrotMinigameManager : MonoBehaviour
{
    [SerializeField]
    private float _timer = 0;

    private bool _GameEnded = false;

    [SerializeField]
    GameObject _pillarParent,
               _pillar,
               _player;

    [SerializeField]
    private float _offScreen; //Used to determin where to spawn and destroy the pillars
    [SerializeField]
    private float _centerPointOffset = 0f; //Used so that the gap wont be at the edges of the screen
    private int _score = 0;
    private List<GameObject> _pillars = new List<GameObject>();

    [SerializeField]
    private float _spaceBetweenPillars = 200.0f;//Space between top and bottom Spillars, not between each set 
    [SerializeField]
    private float _timeBetweenSpawns = 2.0f;
    [SerializeField]
    private float _pillarMoveSpeed = 40.0f;

    [SerializeField]
    private TextMeshPro _scoreText;

    private Vector3 _bottomLeft,//WorldPoint
                    _topRight;//WorldPoint




    private void Start()
    {
        _bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        _topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane));

    }
    // Update is called once per frame
    void Update()
    {
        CheckEndGame();
        if (!_GameEnded)
        {
            UpdateTime();
            if (_timer > _timeBetweenSpawns)//ok
            {
                SpawnPillars();//ok
                _timer = 0;
            }
            MovePillars();//ok
            UpdateScore(); //If were gonna calculate score like that
            DespawnPillars();//ok
        }
    }

    private void CheckEndGame()
    {
        foreach (GameObject pillar in _pillars)
        {
            foreach(Pillar child in pillar.transform)
            {
                if (child._TouchedPlayer)
                {
                    _GameEnded = true;
                }
            }
        }
        Debug.Log(_GameEnded);
    }

    private void UpdateScore()
    {
        foreach (GameObject pillar in _pillars)
        {
            if (pillar.transform.position.x <= _player.transform.position.x)
            {
                _score++;
                //_pillar.
                //foreach (Pillar child in pillar.transform)
                //{
                //    if (!child._WasScored)
                //    {
                //        child._WasScored = true;
                //        _score++;

                //    }
                //    else
                //    {
                //        break; 
                //    }
                //}
            }
        }
    }

    private void SpawnPillars()
    {
        GameObject pillar = SetPillarParent();
        SetpillarChildren(pillar);
        _pillars.Add(pillar);
    }
    private GameObject SetPillarParent()
    {
        float randomY = UnityEngine.Random.Range(_bottomLeft.y + _centerPointOffset + _spaceBetweenPillars / 2,
                                                _topRight.y - _centerPointOffset - _spaceBetweenPillars / 2);
        float spawnX = _topRight.x + _offScreen; //Spawn the pillars offScreen
        GameObject centerPoint = Instantiate(_pillarParent, new Vector3(spawnX, randomY, 0), Quaternion.identity);
        return centerPoint;
    }
    private void SetpillarChildren(GameObject newPillar)
    {
        GameObject topPillar = Instantiate(_pillar, new Vector3(newPillar.transform.position.x, newPillar.transform.position.y + _spaceBetweenPillars / 2, 0), Quaternion.Euler(new Vector3(0, 0, 90)));
        topPillar.transform.SetParent(newPillar.transform);

        GameObject bottomPillar = Instantiate(_pillar, new Vector3(newPillar.transform.position.x, newPillar.transform.position.y - _spaceBetweenPillars / 2, 0), Quaternion.Euler(new Vector3(0, 0, 90)));
        bottomPillar.transform.SetParent(newPillar.transform);
    }
    private void MovePillars()
    {
        foreach (GameObject pillarSet in _pillars)
        {
            pillarSet.transform.position -= new Vector3(_pillarMoveSpeed * Time.deltaTime, 0, 0);
        }
        //foreach (GameObject pillarSet in _pillars)
        //{
        //    pillarSet.transform.position -= new Vector3(_pillarMoveSpeed, 0, 0) * Time.deltaTime;
        //}
    }
    private void DespawnPillars()
    {
        for (int i = _pillars.Count - 1; i >= 0; i--)
        {
            if (_pillars[i].transform.position.x < _bottomLeft.x - _offScreen)
            {
                Destroy(_pillars[i]);
                _pillars.RemoveAt(i);
            }
        }
        //foreach (GameObject pillar in _pillars)
        //{
        //    if (pillar.transform.position.x < _bottomLeft.x - _offScreen)
        //    {
        //        Destroy(pillar);
        //        _pillars.Remove(pillar);
        //    }
        //}
        //for (int i = _pillars.Count - 1; i >= 0; i--)
        //{
        //    if (_pillars[i] == null)
        //    {
        //        _pillars.RemoveAt(i);

        //    }
        //}
    }
    private void UpdateTime()
    {
        _timer += Time.deltaTime;
    }
}
