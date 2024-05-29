using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Spawner : MonoBehaviour
{
    private GameManager gameManager;

    public Spawns levelSpawns;

    void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();

        for (int i = 0; i < levelSpawns.player1Spawns.Length; i++)
        {
            gameManager.player1.crew[i].transform.position = Vector3.zero;
            gameManager.player1.crew[i].transform.position = levelSpawns.player1Spawns[i].transform.position;
            gameManager.player1.crew[i].currentHex = gameManager.player1.crew[i].transform.position.ToHex();
            gameManager.player1.crew[i].previousHex = gameManager.player1.crew[i].currentHex;
            gameManager.player1.crew[i].GetComponent<Node>().ApplyTransform();
        }

        for (int i = 0; i < levelSpawns.player2Spawns.Length; i++)
        {
            gameManager.player1.crew[i].transform.position = Vector3.zero;
            gameManager.player2.crew[i].transform.position = levelSpawns.player2Spawns[i].transform.position;
            gameManager.player2.crew[i].currentHex = gameManager.player2.crew[i].transform.position.ToHex();
            gameManager.player2.crew[i].previousHex = gameManager.player2.crew[i].currentHex;
            gameManager.player2.crew[i].GetComponent<Node>().ApplyTransform();
        }

        for (int i = 0; i < levelSpawns.treasureSpawns.Length; i++)
        {
            gameManager.treasureChests.Add(Instantiate(gameManager.treasurePrefab, levelSpawns.treasureSpawns[i]));
        }
    }
}
