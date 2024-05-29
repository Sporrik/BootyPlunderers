using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameState { START, P1_TURN, P2_TURN, END };

[Serializable]
public struct Player
{
    public int coins;
    public int ammo;
    public int cannonballs;
    public string firstMate;
    public GameObject crewPrefab;

    public Unit[] crew;
    public UI[] HUD;
    public Spawns spawnLocations;

    public int alive;
    public bool isSpecialAvailable;
}

[Serializable]
public struct Spawns
{
    public Transform[] spawns1;
    public Transform[] spawns2;
    public Transform[] spawns3;
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameState state;

    public Player player1;
    public Player player2;

    public GameObject treasurePrefab;
    public List<Transform> treasureSpawns1;
    public List<Transform> treasureSpawns2;
    public List<Transform> treasureSpawns3;
    private List<GameObject> treasureChests;


    private int maxTreasure;
    private int treasureValue = 10;

    private Unit selectedPirate;
    private Unit selectedEnemy;

    public TextMeshProUGUI dialogueText;

    private int selectedAmmo;
    private bool isAttacking = false;

    private Camera mainCam;
    public ShootMinigame attackMinigame;
    public MonkeyMiniGame monkeyMinigame;

    private int minigameCount;
    private int currentLevel = 0;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        player1.isSpecialAvailable = true;
        player2.isSpecialAvailable = true;

        player1.coins = 10;
        player2.coins = 10;

        mainCam = Camera.main;
        mainCam.enabled = false;

        state = GameState.START;

        InitialSetup();
    }

    private void Update()
    {
        if (state == GameState.P1_TURN || state == GameState.P2_TURN)
        {
            UpdateUI(player1.crew, player1.HUD);
            UpdateUI(player2.crew, player2.HUD);
            player1.HUD[0].SetCoins(player1.coins);
            player2.HUD[0].SetCoins(player2.coins);

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

                if (hit.collider && IsThisAnEnemy(hit) && !selectedPirate.hasAttacked)
                {
                    selectedEnemy = hit.collider.GetComponent<Unit>();
                    Attack();
                }
                else if (selectedPirate)
                {
                    selectedPirate.targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    StartCoroutine(selectedPirate.MoveUnit());
                }
            }

            if (selectedPirate != null && selectedPirate.collectedTreasure != 0)
            {
                selectedPirate.collectedTreasure = 0;
                maxTreasure--;

                switch (state)
                {
                    case GameState.P1_TURN:
                        player1.coins += treasureValue;
                        player1.HUD[0].SetCoins(player1.coins);
                        break;
                    case GameState.P2_TURN:
                        player2.coins += treasureValue;
                        player2.HUD[0].SetCoins(player2.coins);
                        break;
                }

                if (maxTreasure == 0)
                {
                    StartCoroutine(EndBattle());
                }
            }

            if (Input.GetKeyDown(KeyCode.Space) && isAttacking)
            {
                StartCoroutine(ResolveAttack());
            }
        } 
    }

    private void InitialSetup()
    {
        maxTreasure = 3;

        player1.crew = new Unit[3];
        player2.crew = new Unit[3];

        player1.alive = player1.crew.Length;
        player2.alive = player2.crew.Length;


        for (int i = 0; i < player1.crew.Length; i++)
        {
            var p2 = Instantiate(player1.crewPrefab, gameObject.transform);
            player1.crew[i] = p2.GetComponent<Unit>();
            player1.crew[i].transform.position = new Vector3(-100, -100, -100);
        }

        for (int i = 0; i < player2.crew.Length; i++)
        {
            var p2 = Instantiate(player2.crewPrefab, gameObject.transform);
            player2.crew[i] = p2.GetComponent<Unit>();
            player2.crew[i].transform.position = new Vector3(-100, -100, -100);
        }
    }

    public void NewLevel()
    {
        switch (currentLevel)
        {
            case 0:
                SceneManager.LoadScene("Map_1");
                break;
            case 1:
                SceneManager.LoadScene("Map_2");
                break;
            case 2:
                SceneManager.LoadScene("Map_3");
                break;
        }

        mainCam.enabled = true;
        currentLevel++;
        StartCoroutine(SetupGame());
    }

    IEnumerator SetupGame()
    {
        UpdateUI(player1.crew, player1.HUD);
        UpdateUI(player2.crew, player2.HUD);
        
        

        for (int i = 0; i < player1.crew.Length; i++)
        {
            player1.HUD[i].SetHUD(player1.crew[i]);
        }

        for (int i = 0; i < player2.crew.Length; i++)
        {
            player2.HUD[i].SetHUD(player2.crew[i]);
        }

        dialogueText.text = "Plunder their Booty";

        yield return new WaitForSeconds(2f);

        state = GameState.P1_TURN;
        P1_Turn();
    }

    private void P1_Turn()
    {
        dialogueText.text = "Player 1 Turn";

        foreach (Unit p in player1.crew)
        {
            p.Reset();
        }
    }

    private void P2_Turn()
    {
        dialogueText.text = "Player 2 Turn";

        foreach (Unit p in player2.crew)
        {
            p.Reset();
        }
    }

    private int CheckRange()
    {
        return selectedPirate.currentHex.DistanceTo(selectedEnemy.transform.position.ToHex());
    }

    void Attack()
    {
        if (state == GameState.P1_TURN)
        {
            selectedAmmo = player1.ammo;
        }
        else if (state == GameState.P2_TURN)
        {
            selectedAmmo = player2.ammo;
        }

        switch (CheckRange())
        {
            case < 1:
                StartCoroutine(CommitAttack());
                break;
            case < 4:
                StartCoroutine(CommitAttack());
                break;
            default:
                dialogueText.text = "Out of Range";
                break;
        }
    }

    IEnumerator CommitAttack()
    {
        isAttacking = true;

        selectedPirate.hasAttacked = true;
        dialogueText.text = "Attack!";

        yield return new WaitForSeconds(2f);

        attackMinigame.gameObject.SetActive(true);
    }

    IEnumerator ResolveAttack()
    {
        bool isDead = selectedEnemy.TakeDamage(attackMinigame.CheckCollision());

        if (isDead)
        {
            if (selectedEnemy.heldObject != null)
            {
                selectedEnemy.heldObject.transform.localScale = new Vector2(1f, 1f);
                selectedEnemy.heldObject.transform.SetParent(this.transform, true);
                selectedEnemy.heldObject.transform.position -= Vector3.up * 0.5f;
            }

            selectedEnemy.gameObject.transform.position = new Vector3(-100, -100, -100);

            switch (state)
            {
                case GameState.P1_TURN:
                    player2.alive--;
                    break;
                case GameState.P2_TURN:
                    player1.alive--;
                    break;
            }
        }

        if (player1.alive == 0 || player2.alive == 0)
        {
            StartCoroutine(EndBattle());
        }

        yield return new WaitForSeconds(2f);

        attackMinigame.gameObject.SetActive(false);
        isAttacking = false;

        if (state == GameState.P1_TURN)
        {
            dialogueText.text = "Player 1 Turn";
        }
        else if (state == GameState.P2_TURN)
        {
            dialogueText.text = "Player 2 Turn";
        }
    }

    IEnumerator EndBattle()
    {
        dialogueText.text = "Level Complete";
        state = GameState.END;

        switch (state)
        { 
            case GameState.P1_TURN:
                player1.coins += maxTreasure * treasureValue;
                player1.HUD[0].SetCoins(player1.coins);
                dialogueText.text = "Player 1 Wins";
                break;
            case GameState.P2_TURN:
                player2.coins += maxTreasure * treasureValue;
                player2.HUD[0].SetCoins(player2.coins);
                dialogueText.text = "Player 2 Wins";
                break;
        }

        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene("Island");
    }

    public void EndTurnButton()
    {
        selectedPirate = null;
        selectedEnemy = null;

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

    public void SpecialAttackP1()
    {
        if (!player1.isSpecialAvailable || state != GameState.P1_TURN) return;

        player1.isSpecialAvailable = false;

        switch (player1.firstMate)
        {
            case "Banana Joe":
                monkeyMinigame.gameObject.SetActive(true);
                monkeyMinigame.GetComponentInChildren<Camera>().enabled = true;
                break;
            case "Parrotmancer":
                break;
            case "The Doctor":
                break;
        }
        
        mainCam.enabled = false;
    }

    public void SpecialAttackP2()
    {
        if (!player2.isSpecialAvailable || state != GameState.P2_TURN) return;

        player2.isSpecialAvailable = false;

        switch (player2.firstMate)
        {
            case "Banana Joe":
                monkeyMinigame.gameObject.SetActive(true);
                monkeyMinigame.GetComponentInChildren<Camera>().enabled = true;
                break;
            case "Parrotmancer":
                break;
            case "The Doctor":
                break;
        }

        mainCam.enabled = false;
    }

    private bool DoesThisBelongToYou(RaycastHit2D hit)
    {
        return ((state == GameState.P1_TURN && hit.collider.gameObject.CompareTag("P1_Crew")) || (state == GameState.P2_TURN && hit.collider.gameObject.CompareTag("P2_Crew")));
    }

    private bool IsThisAnEnemy(RaycastHit2D hit)
    {
        return ((state == GameState.P1_TURN && hit.collider.gameObject.CompareTag("P2_Crew")) || (state == GameState.P2_TURN && hit.collider.gameObject.CompareTag("P1_Crew")));
    }

    private void UpdateUI(Unit[] unitArray, UI[] uiArray)
    {
        for (int i = 0; i < unitArray.Length; i++)
        {
            uiArray[i].SetHP(unitArray[i].currentHealth);
            uiArray[i].SetMove(unitArray[i].movement);
        }
    }

    public void EndMinigame(int score)
    {
        minigameCount = score;
        
        mainCam.enabled = true;

        switch (state)
        {
            case GameState.P1_TURN:
                switch (player1.firstMate)
                {
                    case "Banana Joe":
                        monkeyMinigame.gameObject.SetActive(false);
                        monkeyMinigame.GetComponentInChildren<Camera>().enabled = false;

                        player1.coins += score;
                        player1.HUD[0].SetCoins(player1.coins);

                        break;
                    case "Parrotmancer":
                        //disable parrot minigame
                        break;
                    case "The Doctor":
                        //Disable booze minigame
                        break;
                }
                break;
            case GameState.P2_TURN:
                switch (player2.firstMate)
                {
                    case "Banana Joe":
                        monkeyMinigame.gameObject.SetActive(false);
                        monkeyMinigame.GetComponentInChildren<Camera>().enabled = false;

                        player2.coins += score;
                        player2.HUD[0].SetCoins(player2.coins);

                        break;
                    case "Parrotmancer":
                        //disable parrot minigame
                        break;
                    case "The Doctor":
                        //Disable booze minigame
                        break;
                }
                break;
        }
    }

    private void SpawnObjects()
    {
        switch (currentLevel)
        {
            case 0:
                for (int i = 0; i < player1.crew.Length; i++)
                {
                    player1.crew[i].transform.position = player1.spawnLocations.spawns1[i].position;
                    player1.crew[i].currentHex = player1.crew[i].transform.position.ToHex();
                    player1.crew[i].previousHex = player1.crew[i].currentHex;
                }

                for (int i = 0; i < player1.crew.Length; i++)
                {
                    player2.crew[i].transform.position = player2.spawnLocations.spawns1[i].position;
                    player2.crew[i].currentHex = player2.crew[i].transform.position.ToHex();
                    player2.crew[i].previousHex = player2.crew[i].currentHex;
                }

                for (int i = 0; i < treasureSpawns1.Count; i++)
                {
                    treasureChests.Add(Instantiate(treasurePrefab, treasureSpawns1[i]));
                }
                break;
            case 1:
                for (int i = 0; i < player1.crew.Length; i++)
                {
                    player1.crew[i].transform.position = player1.spawnLocations.spawns2[i].position;
                    player1.crew[i].currentHex = player1.crew[i].transform.position.ToHex();
                    player1.crew[i].previousHex = player1.crew[i].currentHex;
                }

                for (int i = 0; i < player1.crew.Length; i++)
                {
                    player2.crew[i].transform.position = player2.spawnLocations.spawns2[i].position;
                    player2.crew[i].currentHex = player2.crew[i].transform.position.ToHex();
                    player2.crew[i].previousHex = player2.crew[i].currentHex;
                }

                for (int i = 0; i < treasureSpawns1.Count; i++)
                {
                    treasureChests.Add(Instantiate(treasurePrefab, treasureSpawns2[i]));
                }
                break;
            case 2:
                for (int i = 0; i < player1.crew.Length; i++)
                {
                    player1.crew[i].transform.position = player1.spawnLocations.spawns3[i].position;
                    player1.crew[i].currentHex = player1.crew[i].transform.position.ToHex();
                    player1.crew[i].previousHex = player1.crew[i].currentHex;
                }

                for (int i = 0; i < player1.crew.Length; i++)
                {
                    player2.crew[i].transform.position = player2.spawnLocations.spawns3[i].position;
                    player2.crew[i].currentHex = player2.crew[i].transform.position.ToHex();
                    player2.crew[i].previousHex = player2.crew[i].currentHex;
                }

                for (int i = 0; i < treasureSpawns1.Count; i++)
                {
                    treasureChests.Add(Instantiate(treasurePrefab, treasureSpawns3[i]));
                }
                break;
        }
    }
}
