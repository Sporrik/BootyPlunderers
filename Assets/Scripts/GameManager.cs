using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum GameState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class GameManager : MonoBehaviour
{
    public GameState state;

    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public GameObject treasurePrefab;

    public Transform playerSpawn;
    public Transform enemySpawn;
    public Transform treasureSpawnA;
    public Transform treasureSpawnB;
    public Transform treasureSpawnC;

    private PirateCrew playerUnit;
    private EnemyCrew enemyUnit;

    public TextMeshProUGUI dialogueText;

    public UI playerHUD;
    public UI enemyHUD;

    private int maxTreasure = 3;

    private void Start()
    {
        state = GameState.START;
        StartCoroutine(SetupGame());
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse1) && playerUnit._movement > 0 && state == GameState.PLAYERTURN)
        {
            playerUnit._targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            StartCoroutine(playerUnit.MoveCharacter()); 
        }

        if (Input.GetKeyUp(KeyCode.Mouse0) && !playerUnit.hasAttacked && state == GameState.PLAYERTURN)
        {
            playerUnit._attackTarget = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            StartCoroutine(PlayerAttack());
        }

        playerHUD.SetMove(playerUnit._movement);

        if (playerUnit.collectedTreasure == maxTreasure)
        {
            state = GameState.WON;
            EndBattle();
        }
    }

    IEnumerator SetupGame()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerSpawn);
        playerUnit = playerGO.GetComponent<PirateCrew>();

        GameObject enemyGO = Instantiate(enemyPrefab, enemySpawn);
        enemyUnit = enemyGO.GetComponent<EnemyCrew>();

        Instantiate(treasurePrefab, treasureSpawnA);
        Instantiate(treasurePrefab, treasureSpawnB);
        Instantiate(treasurePrefab, treasureSpawnC);

        dialogueText.text = "Plunder their Booty!";

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        yield return new WaitForSeconds(2f);

        state = GameState.PLAYERTURN;
        StartCoroutine(PlayerTurn());
    }

    IEnumerator PlayerTurn()
    {
        dialogueText.text = "Your Turn";

        playerUnit.hasAttacked = false;
        playerUnit._movement = playerUnit._moveSpeed;
        playerHUD.SetMove(playerUnit._movement);

        yield return new WaitForSeconds(2f);
    }

    IEnumerator PlayerAttack()
    {
        if (playerUnit._currentHex.DistanceTo(playerUnit._attackTarget.ToHex()) < 4)
        {
            if (playerUnit._attackTarget.ToHex().DistanceTo(enemyUnit._currentHex) == 0)
            {
                playerUnit.hasAttacked = true;

                bool isDead = enemyUnit.TakeDamage(playerUnit._damage);
                enemyHUD.SetHP(enemyUnit._currentHealth);

                dialogueText.text = "Attack!";

                yield return new WaitForSeconds(2f);

                if (isDead)
                {
                    state = GameState.WON;
                    EndBattle();
                }
            }
            else
            {
                dialogueText.text = "No Target";
            }
        }
        else
        {
            dialogueText.text = "Out of Range";
        }

        yield return new WaitForSeconds(2f);
        dialogueText.text = "Your Turn";
    }

    IEnumerator EnemyTurn()
    {
        enemyUnit._movement = enemyUnit._moveSpeed;

        enemyUnit._targetPosition = playerUnit.transform.position;

        if (enemyUnit._currentHex.DistanceTo(enemyUnit._targetPosition.ToHex()) > 1)
        {
            StartCoroutine(enemyUnit.MoveEnemy());
        }

        if (enemyUnit._currentHex.DistanceTo(enemyUnit._targetPosition.ToHex()) < 4)
        {
            dialogueText.text = "Enemy Attacks!";

            yield return new WaitForSeconds(2f);

            bool isDead = playerUnit.TakeDamage(enemyUnit._damage);

            playerHUD.SetHP(playerUnit._currentHealth);

            yield return new WaitForSeconds(2f);

            if (isDead)
            {
                state = GameState.LOST;
                EndBattle();
            }
            else
            {
                state = GameState.PLAYERTURN;
                StartCoroutine(PlayerTurn());
            }
        }

        state = GameState.PLAYERTURN;
        StartCoroutine(PlayerTurn());
    }

    void EndBattle()
    {
        if (state == GameState.WON)
        {
            dialogueText.text = "Victory!";
        }
        else if (state == GameState.LOST)
        {
            dialogueText.text = "Defeat!";
        }
    }

    public void EndTurnButton()
    {
        if (state != GameState.PLAYERTURN) return;

        state = GameState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }
}


