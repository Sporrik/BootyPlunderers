using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BoozeMiniGame : MonoBehaviour
{
    [SerializeField] Camera mainCamera;

    public GameObject bottlePrefab, bombPrefab;
    private Vector3 _bottomLeftCorner, _topRightCorner;

    private int _maxBottlesInScene = 15, _score, _scorePerBottle = 5, _timeOfRound = 10,
                _maxBombsInScene = 10;
    private int _bottlesInScene,
                _bombsInScene;

    public List<GameObject> bottleList, bombList;

    private GameObject _heldObject;

    [SerializeField] Collider2D pirateMouth;

    [SerializeField] TextMeshProUGUI scoreText, timerText, finalScoreText;
    [SerializeField] GameObject gameOverPanel;

    private float _bombTimer, _timer;
    private bool _isCounting = true;

    public GameManager gameManager;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _timer = _timeOfRound;
        DisplayScore();
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

    private void Update()
    {
        if (_isCounting)
        {
            _timer -= Time.deltaTime;
            timerText.text = Mathf.Ceil(_timer).ToString();
        }
        if (_timer > 0)
        {
            DisplayScore();
            CheckMouseCollision();
            CheckCollisionObject();
        }
        else if (_timer <= _timeOfRound )
        {
            _isCounting = false;
            StartCoroutine(EndGame());
        }
    }

    IEnumerator EndGame()
    {
        gameOverPanel.gameObject.SetActive(true);
        _heldObject.GetComponent<BoozeMiniGameMovement>().isMoving = true;
        if (_score < 0)
        {
            finalScoreText.text = $"0 HP";
        }
        else
        {
            finalScoreText.text = $"{_score} HP";
        }

        yield return new WaitForSeconds(2f);

        gameManager.EndMinigame(_score);
    }

    private void CheckCollisionObject()
    {
        if (_heldObject && _heldObject.CompareTag("Booze"))
        {
            if (Input.GetMouseButton(0))
            {
                MoveHeldObject();
            }
            else if (_heldObject.GetComponent<Collider2D>().IsTouching(pirateMouth) && Input.GetMouseButtonUp(0))
            {
                _score += _scorePerBottle;
                Destroy(_heldObject);
                _heldObject = null;
            }
            else
            {
                _heldObject.GetComponent<BoozeMiniGameMovement>().isMoving = true;
                _heldObject = null;
            }
        }
        else if (_heldObject && _heldObject.CompareTag("Bomb"))
        {
            _bombTimer += Time.deltaTime;
            _heldObject.GetComponent<SpriteRenderer>().color = Color.red;

            if (_bombTimer >= .2f)
            {
                _score -= _scorePerBottle;                
                Destroy(_heldObject);
                _heldObject = null;
            }
        }
    }

    private void DisplayScore()
    {        
        scoreText.text = $"{_score}";
    }

    private void CheckMouseCollision()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider)
            {
                if (hit.collider.TryGetComponent<BoozeMiniGameMovement>(out BoozeMiniGameMovement movementScript))
                {
                    movementScript.isMoving = false;
                    _heldObject = hit.collider.gameObject;
                }                
            }
        }
    }

    private void MoveHeldObject()
    {
        var cameraPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        _heldObject.transform.position = new Vector3(cameraPosition.x,cameraPosition.y, transform.position.z);
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
}
