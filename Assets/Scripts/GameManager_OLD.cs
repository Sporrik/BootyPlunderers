using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager_OLD : MonoBehaviour
{
    //public GameState state;
    //
    //public GameObject playerPrefab;
    //public GameObject enemyPrefab;
    //public GameObject treasurePrefab;
    //
    //public Transform playerSpawn;
    //public Transform enemySpawn;
    //public Transform treasureSpawnA;
    //public Transform treasureSpawnB;
    //public Transform treasureSpawnC;
    //
    //private Unit playerUnitA;
    //private Unit playerUnitB;
    //private Unit playerUnitC;
    //private Unit enemyUnitA;
    //private Unit enemyUnitB;
    //private Unit enemyUnitC;
    //
    //private Unit playerSelected;
    //private Unit enemySelected;
    //public Unit[] turnOrder;
    //
    //public TextMeshProUGUI dialogueText;
    //
    //public UI playerHUD;
    //public UI enemyHUD;
    //
    //private int maxTreasure = 3;
    //
    //private void Start()
 //   {
 //       state = GameState.START;
 //       StartCoroutine(SetupGame());
 //   }
    //
    //private void Update()
   // {
   //     if (Input.GetKeyUp(KeyCode.Mouse1) && playerSelected.movement > 0 && state == GameState.PLAYERTURN)
   //     {
   //         playerSelected.targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
   //         StartCoroutine(playerSelected.MoveUnit()); 
   //     }
   //
   //     if (Input.GetKeyUp(KeyCode.Mouse0) && !playerSelected.hasAttacked && state == GameState.PLAYERTURN)
   //     {
   //         playerSelected.attackTarget = Camera.main.ScreenToWorldPoint(Input.mousePosition);
   //         StartCoroutine(PlayerAttack());
   //     }
   //
   //     playerHUD.SetMove(playerSelected.movement);
   //
   //     if (playerSelected.collectedTreasure == maxTreasure)
   //     {
   //         state = GameState.WON;
   //         EndBattle();
   //     }
   // }
   // //
    //IEnumerator SetupGame()
    //{
    //    GameObject playerGO_A = Instantiate(playerPrefab, playerSpawn);
    //    playerUnitA = playerGO_A.GetComponent<Unit>();
    //    GameObject playerGO_B = Instantiate(playerPrefab, playerSpawn);
    //    playerUnitB = playerGO_B.GetComponent<Unit>();
    //    GameObject playerGO_C = Instantiate(playerPrefab, playerSpawn);
    //    playerUnitC = playerGO_C.GetComponent<Unit>();
    //
    //
    //    GameObject enemyGO_A = Instantiate(enemyPrefab, enemySpawn);
    //    enemyUnitA = enemyGO_A.GetComponent<Unit>();
    //    GameObject enemyGO_B = Instantiate(enemyPrefab, enemySpawn);
    //    enemyUnitB = enemyGO_B.GetComponent<Unit>();
    //    GameObject enemyGO_C = Instantiate(enemyPrefab, enemySpawn);
    //    enemyUnitC = enemyGO_C.GetComponent<Unit>();
    //
    //    Instantiate(treasurePrefab, treasureSpawnA);
    //    Instantiate(treasurePrefab, treasureSpawnB);
    //    Instantiate(treasurePrefab, treasureSpawnC);
    //
    //    dialogueText.text = "Plunder their Booty!";
    //
    //    playerHUD.SetHUD(playerSelected);
    //    enemyHUD.SetHUD(enemySelected);
    //
    //    yield return new WaitForSeconds(2f);
    //
    //    //Set turn order
    //    playerUnitA.initiative = Random.Range(1, 10);
    //    playerUnitB.initiative = Random.Range(1, 10);
    //    playerUnitC.initiative = Random.Range(1, 10);
    //
    //    enemyUnitA.initiative = Random.Range(1, 10);
    //    enemyUnitB.initiative = Random.Range(1, 10);
    //    enemyUnitC.initiative = Random.Range(1, 10);
    //
    //    
    //
    //    state = GameState.PLAYERTURN;
    //    StartCoroutine(PlayerTurn());
    //}
    //
    //IEnumerator PlayerTurn()
    //{
    //    dialogueText.text = "Your Turn";
    //
    //    playerSelected.hasAttacked = false;
    //    playerSelected.movement = playerSelected.moveSpeed;
    //
    //    playerHUD.SetHP(playerSelected.movement);
    //    playerHUD.SetMove(playerSelected.movement);
    //
    //    yield return new WaitForSeconds(2f);
    //}
    //
    //IEnumerator PlayerAttack()
    //{
    //    if (playerSelected.currentHex.DistanceTo(playerSelected.attackTarget.ToHex()) < 4)
    //    {
    //        if (playerSelected.attackTarget.ToHex().DistanceTo(enemySelected.currentHex) == 0)
    //        {
    //            playerSelected.hasAttacked = true;
    //
    //            bool isDead = enemySelected.TakeDamage(playerSelected.damage);
    //            enemyHUD.SetHP(enemySelected.currentHealth);
    //
    //            dialogueText.text = "Attack!";
    //
    //            yield return new WaitForSeconds(2f);
    //
    //            if (isDead)
    //            {
    //                enemySelected.gameObject.SetActive(false);
    //            }
    //        }
    //    }
    //    else
    //    {
    //        dialogueText.text = "Out of Range";
    //    }
    //
    //    yield return new WaitForSeconds(2f);
    //    dialogueText.text = "Your Turn";
    //}
    //
    //IEnumerator EnemyTurn()
    //{
    //    enemySelected.movement = enemySelected.moveSpeed;
    //
    //    enemySelected.targetPosition = playerSelected.transform.position;
    //
    //    if (enemySelected.currentHex.DistanceTo(enemySelected.targetPosition.ToHex()) > 1)
    //    {
    //        StartCoroutine(enemySelected.MoveUnit());
    //    }
    //
    //    if (enemySelected.currentHex.DistanceTo(enemySelected.targetPosition.ToHex()) < 4)
    //    {
    //        dialogueText.text = "Enemy Attacks!";
    //
    //        yield return new WaitForSeconds(2f);
    //
    //        bool isDead = playerSelected.TakeDamage(enemySelected.damage);
    //
    //        playerHUD.SetHP(playerSelected.currentHealth);
    //
    //        yield return new WaitForSeconds(2f);
    //
    //        if (isDead)
    //        {
    //            playerSelected.gameObject.SetActive(false);
    //        }
    //        else
    //        {
    //            state = GameState.PLAYERTURN;
    //            StartCoroutine(PlayerTurn());
    //        }
    //    }
    //
    //    state = GameState.PLAYERTURN;
    //    StartCoroutine(PlayerTurn());
    //}
    //
    //void EndBattle()
    //{
    //    if (state == GameState.WON)
    //    {
    //        dialogueText.text = "Victory!";
    //    }
    //    else if (state == GameState.LOST)
    //    {
    //        dialogueText.text = "Defeat!";
    //    }
    //}
    //
    //public void EndTurnButton()
    //{
    //    if (state != GameState.PLAYERTURN) return;
    //
    //    state = GameState.ENEMYTURN;
    //    StartCoroutine(EnemyTurn());
    //}
}


