using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    public UI[] p1HUD;
    public UI[] p2HUD;
    public TextMeshProUGUI dialogueText;

    private int p1Ammo;
    private int p2Ammo;
    private int selectedAmmo;

    private int maxTreasure;

    private bool isAttacking = false;
    private bool p1SpecialAvailable;
    private bool p2SpecialAvailable;

    private Camera mainCam;
    public ShootMinigame attackMinigame;
    public MonkeyMiniGame monkeyMinigame;

    private string p1FirstMateMinigame;
    private string p2FirstMateMinigame;
    private int minigameCount;

    public int sceneCount;

    private int currentLevel;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        p1FirstMateMinigame = "MonkeyMiniGame";
        p2FirstMateMinigame = "MonkeyMiniGame";

        p1SpecialAvailable = true;
        p2SpecialAvailable = true;

        p1Spawns = new Transform[3];
        p2Spawns = new Transform[3];

        currentLevel = 1;

        state = GameState.START;
        StartCoroutine(SetupGame());
    }

    private void Update()
    {

        sceneCount = SceneManager.loadedSceneCount;

        if (state != GameState.START)
        {
            UpdateUI(p1Crew, p1HUD);
            UpdateUI(p2Crew, p2HUD);
        }

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
                if (IsThisAnEnemy(hit) && !selectedPirate.hasAttacked)
                {
                    selectedEnemy = hit.collider.GetComponent<Unit>();
                    Attack();
                }
            }
            else if (selectedPirate)
            {
                selectedPirate.targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                StartCoroutine(selectedPirate.MoveUnit());
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && isAttacking)
        {
            StartCoroutine(ResolveAttack());
        }
    }

    IEnumerator SetupGame()
    {
        mainCam = Camera.main;

        for (int i = 0; i < p1Spawns.Length; i++)
        {
            p1Spawns[i] = GameObject.Find("p1Spawn" + i).transform;
        }

        for (int i = 0; i < p2Spawns.Length; i++)
        {
            p2Spawns[i] = GameObject.Find("p2Spawn" + i).transform;
        }

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

        for (int i = 0; i < p1Crew.Length; i++)
        {
            p1HUD[i].SetHUD(p1Crew[i]);
        }

        for (int i = 0; i < p2Crew.Length; i++)
        {
            p2HUD[i].SetHUD(p2Crew[i]);
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
            p.Reset();
        }
    }

    private void P2_Turn()
    {
        dialogueText.text = "Player 2 Turn";

        foreach (Unit p in p2Crew)
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
            selectedAmmo = p1Ammo;
        }
        else if (state == GameState.P2_TURN)
        {
            selectedAmmo = p2Ammo;
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
            selectedEnemy.gameObject.SetActive(false);
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

    public void EndBattle()
    {
        //end battle
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
        if (!p1SpecialAvailable || state != GameState.P1_TURN) return;

        p1SpecialAvailable = false;

        monkeyMinigame.gameObject.SetActive(true);
        monkeyMinigame.GetComponentInChildren<Camera>().enabled = true;
        mainCam.enabled = false;
    }

    public void SpecialAttackP2()
    {
        if (!p1SpecialAvailable || state != GameState.P2_TURN) return;

        p2SpecialAvailable = false;

        monkeyMinigame.gameObject.SetActive(true);
        monkeyMinigame.GetComponentInChildren<Camera>().enabled = true;
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

        monkeyMinigame.gameObject.SetActive(true);
        
        mainCam.enabled = true;

        switch (p1FirstMateMinigame)
        {
            case "monkeyMinigame":
                monkeyMinigame.gameObject.SetActive(false);
                monkeyMinigame.GetComponentInChildren<Camera>().enabled = true;
                break;
            case "parrotMinigame":
                //disable parrot minigame
                break;
            case "boozeMinigame":
                //Disable booze minigame
                break;
            default:
                break;
        }
    }
}
