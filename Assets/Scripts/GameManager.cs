using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    public BoardManager boardScript;
    public int playerFoodPoints = 100;
    public float turnDelay = .1f;
    public float levelStartDelay = 5f;

    [HideInInspector]
    public bool playersTurn = true;

    private int level = 1;
    private List<Enemy> enemies;
    private bool enemiesMoving;
    private bool doingSetup;
    private Text levelText;
    private GameObject levelImage;

	// Use this for initialization
	void Awake () {
        if (instance == null){
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        boardScript = GetComponent<BoardManager>();
        enemies = new List<Enemy>();
       
        InitGame();
	}

    private void OnLevelWasLoaded(int index)
    {
        level++;
        InitGame();
    }

    private void HideLevelImage() {        
        levelImage.SetActive(false);
        doingSetup = false;
    }

    void InitGame() {
        doingSetup = true;

        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = "Day " + level;        
        levelImage.SetActive(true);        
        Invoke("HideLevelImage", levelStartDelay);

        enemies.Clear();
        boardScript.SetupScene(level);
    }
	
    public void GameOver() {
        enabled = false;
        levelText.text = "After " + level + " days, you starved.";
        levelImage.SetActive(true);
    }

	// Update is called once per frame
	void Update () {
        if(playersTurn || enemiesMoving || doingSetup) {
            return;
        }
        StartCoroutine(MoveEnemies());
	}

    public void AddEnemyToList(Enemy e) {
        enemies.Add(e);
    }

    IEnumerator MoveEnemies() {
        enemiesMoving = true;
        yield return new WaitForSeconds(turnDelay);
        if(enemies.Count ==0 ) {
            yield return new WaitForSeconds(turnDelay);
        }
        for (int i = 0; i < enemies.Count; i++) {
            enemies[i].MoveEnemy();
            yield return new WaitForSeconds(enemies[i].moveTime);
        }
        playersTurn = true;
        enemiesMoving = false;
    }
}
