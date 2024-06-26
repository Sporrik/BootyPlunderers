using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private GameManager gameManager;

    public Spawns levelSpawns;

    void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();

        for (int i = 0; i < levelSpawns.player1Spawns.Length; i++)
        {
            gameManager.player1.crew[i].transform.position = levelSpawns.player1Spawns[i].transform.position;
            gameManager.player1.crew[i].currentHex = gameManager.player1.crew[i].transform.position.ToHex();
            gameManager.player1.crew[i].previousHex = gameManager.player1.crew[i].currentHex;
            gameManager.player1.crew[i].targetPosition = levelSpawns.player1Spawns[i].transform.position;
            StartCoroutine(gameManager.player1.crew[i].MoveUnit());
        }

        for (int i = 0; i < levelSpawns.player2Spawns.Length; i++)
        {
            gameManager.player2.crew[i].transform.position = levelSpawns.player2Spawns[i].transform.position;
            gameManager.player2.crew[i].currentHex = gameManager.player2.crew[i].transform.position.ToHex();
            gameManager.player2.crew[i].previousHex = gameManager.player2.crew[i].currentHex;
            gameManager.player2.crew[i].targetPosition = levelSpawns.player2Spawns[i].transform.position;
            StartCoroutine(gameManager.player2.crew[i].MoveUnit());
        }

        for (int i = 0; i < levelSpawns.treasureSpawns.Length; i++)
        {
            gameManager.treasureChests.Add(Instantiate(gameManager.treasurePrefab, levelSpawns.treasureSpawns[i]));
        }
    }
}