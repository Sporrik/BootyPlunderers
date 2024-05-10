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
    public Transform treasureSpawn;

    private PirateCrew playerUnit;
    private EnemyCrew enemyUnit;

    public TextMeshProUGUI dialogueText;

    public UI playerHUD;
    public UI enemyHUD;

    private void Start()
    {
        state = GameState.START;
        StartCoroutine(SetupGame());
    }

    IEnumerator SetupGame()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerSpawn);
        playerUnit = playerGO.GetComponent<PirateCrew>();

        GameObject enemyGO = Instantiate(enemyPrefab, enemySpawn);
        enemyUnit = enemyGO.GetComponent<EnemyCrew>();

        Instantiate(treasurePrefab, treasureSpawn);

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
        yield return new WaitForSeconds(2f);
    }

    public void OnAttackButton()
    {
        if (state != GameState.PLAYERTURN) return;

        StartCoroutine(PlayerAttack());
    }

    IEnumerator PlayerAttack()
    {
        bool isDead = enemyUnit.TakeDamage(playerUnit._damage);

        enemyHUD.SetHP(enemyUnit._currentHealth);
        dialogueText.text = "Attack!";

        yield return new WaitForSeconds(2f);

        if (isDead)
        {
            state = GameState.WON;
            EndBattle();
        }
        else
        {
            state = GameState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator EnemyTurn()
    {
        dialogueText.text = "Enemy Attacks!";

        yield return new WaitForSeconds(1f);

        bool isDead = playerUnit.TakeDamage(enemyUnit._damage);

        playerHUD.SetHP(playerUnit._currentHealth);

        yield return new WaitForSeconds(1f);

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
}


