using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;

public enum GameState { START, P1_TURN, P2_TURN, END };

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameState state;

    public GameObject p1CrewPrefab;
    public GameObject p2CrewPrefab;
    public GameObject treasurePrefab;

    public Transform[] p1Spawns;
    public Transform[] p2Spawns;
    public Transform[] treasureSpawns;

    private Unit[] p1Crew;
    private Unit[] p2Crew;
    private GameObject[] treasureChests;

    private Unit selectedPirate;
    private Unit selectedEnemy;

    public TextMeshProUGUI dialogueText;

    //public UI p1HUD;
    //public UI p2HUD;

    private int maxTreasure;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        state = GameState.START;
        StartCoroutine(SetupGame());
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider)
            {
                if (DoesThisBelongToYou(hit))
                {
                    selectedPirate = hit.collider.GetComponent<Unit>();
                }
            }            
        }

        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider)
            {
                if (IsThisAnEnemy(hit))
                {
                    selectedEnemy = hit.collider.GetComponent<Unit>();
                    Attack();
                }
            }
            else
            {
                selectedPirate.targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                StartCoroutine(selectedPirate.MoveUnit());
            }
        }
    }

    IEnumerator SetupGame()
    {
        p1Crew = new Unit[p1Spawns.Length];
        for (int i = 0; i < p1Spawns.Length; i++)
        {
            var p1 = Instantiate(p1CrewPrefab, p1Spawns[i]);
            p1Crew[i] = p1.GetComponent<Unit>();
        }

        p2Crew = new Unit[p2Spawns.Length];
        for (int i = 0; i < p2Spawns.Length; i++)
        {
            var p2 = Instantiate(p2CrewPrefab, p2Spawns[i]);
            p2Crew[i] = p2.GetComponent<Unit>();
        }

        treasureChests = new GameObject[treasureSpawns.Length];
        for (int i = 0; i < treasureSpawns.Length; i++)
        {
            treasureChests[i] = Instantiate(treasurePrefab, treasureSpawns[i]);
        }

        dialogueText.text = "Plunder their Booty";

        yield return new WaitForSeconds(2f);

        state = GameState.P1_TURN;
        P1_Turn();
    }

    private void P1_Turn()
    {
        dialogueText.text = "Player 1 Turn";

        foreach (Unit p in p1Crew)
        {
            //reset crew stats
        }
    }

    private void P2_Turn()
    {
        dialogueText.text = "Player 2 Turn";

        foreach (Unit p in p2Crew)
        {
            //reset crew stats
        }
    }

    private bool IsInRange()
    {
        return (selectedPirate.currentHex.DistanceTo(selectedEnemy.transform.position.ToHex()) < 4);
    }


    IEnumerator Attack()
    {
        if (IsInRange())
        {
                selectedPirate.hasAttacked = true;
    
                bool isDead = selectedEnemy.TakeDamage(selectedPirate.damage);
                //enemyHUD.SetHP(enemySelected.currentHealth);
    
                dialogueText.text = "Attack!";
    
                yield return new WaitForSeconds(2f);
    
                if (isDead)
                {
                    selectedEnemy.gameObject.SetActive(false);
                }
        }
        else
        {
            dialogueText.text = "Out of Range";
        }
    
        yield return new WaitForSeconds(2f);
        dialogueText.text = "Your Turn";
    }

    public void EndBattle()
    {
        //end battle
    }

    public void EndTurnButton()
    {
        switch (state)
        {
            case GameState.P1_TURN:
                state = GameState.P2_TURN;
                P2_Turn();
                break;
            case GameState.P2_TURN:
                state = GameState.P1_TURN;
                P1_Turn();
                break;
        }
    }

    private bool DoesThisBelongToYou(RaycastHit2D hit)
    {
        return ((state == GameState.P1_TURN && hit.collider.gameObject.CompareTag("P1_Crew")) || (state == GameState.P2_TURN && hit.collider.gameObject.CompareTag("P2_Crew")));
    }

    private bool IsThisAnEnemy(RaycastHit2D hit)
    {
        return ((state == GameState.P1_TURN && hit.collider.gameObject.CompareTag("P2_Crew")) || (state == GameState.P2_TURN && hit.collider.gameObject.CompareTag("P1_Crew")));
    }
}
