using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class GameManager : MonoBehaviour
{
    public GameState state;

    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public GameObject treasurePrefab;

    public Transform playerSpawn;
    public Transform enemySpawn;
    public Transform treasureSpawn;

    private PirateCrew playerUnit;
    private EnemyCrew enemyUnit;

    private void Start()
    {
        state = GameState.START;
        SetupGame();
    }

    private void SetupGame()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerSpawn);
        playerUnit = playerGO.GetComponent<PirateCrew>();

        GameObject enemyGO = Instantiate(enemyPrefab, enemySpawn);
        enemyUnit = enemyGO.GetComponent<EnemyCrew>();

        Instantiate(treasurePrefab, treasureSpawn);
    }
}


