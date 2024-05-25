using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

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
    private int _centerPointOffset; //Used so that the gapwont be at the edges of the screen
    private int _score = 0;
    private List<GameObject> _pillars = new List<GameObject>();

    [SerializeField]
    private float _spaceBetweenPillars = 500.0f;//Space between top and bottom pillars, not between each set 
    [SerializeField]
    private float _timeBetweenSpawns = 2.0f;
    [SerializeField]
    private float _pillarMoveSpeed = 40.0f;


    // Update is called once per frame
    void Update()
    {
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

    private void UpdateScore()
    {
        foreach (GameObject pillar in _pillars)
        {
            if (pillar.transform.position.x == _player.transform.position.x)
            {
                _score++;
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
        float randomY = UnityEngine.Random.Range(0 + _centerPointOffset + _spaceBetweenPillars / 2,
                                                 Screen.height - _centerPointOffset - _spaceBetweenPillars / 2);
        GameObject centerPoint = Instantiate(_pillarParent, new Vector3(Screen.width + _offScreen, randomY, 0), Quaternion.identity);
        return centerPoint;
    }
    private void SetpillarChildren(GameObject newPillar)
    {
        GameObject topPillar = Instantiate(_pillar, new Vector3(newPillar.transform.position.x, newPillar.transform.position.y + _spaceBetweenPillars / 2, 0), Quaternion.Euler(new Vector3(0,0,90)));
        topPillar.transform.SetParent(newPillar.transform);


        GameObject bottomPillar = Instantiate(_pillar, new Vector3(newPillar.transform.position.x, newPillar.transform.position.y - _spaceBetweenPillars / 2, 0), Quaternion.Euler(new Vector3(0, 0, 90)));
        bottomPillar.transform.SetParent(newPillar.transform);
    }
    private void MovePillars()
    {
        foreach (GameObject pillarSet in _pillars)
        {
            pillarSet.transform.position -= new Vector3(_pillarMoveSpeed, 0, 0)*Time.deltaTime;
        }
    }
    private void DespawnPillars()
    {
        foreach (GameObject pillar in _pillars)
        {
            if (pillar.transform.position.x < 0 - _offScreen)
            {
                Destroy(pillar.transform);
            }
        }
    }

    private void UpdateTime()
    {
        _timer += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _GameEnded = true;
        }
    }


}
