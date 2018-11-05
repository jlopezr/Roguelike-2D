﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    public BoardManager boardScript;
    public int playerFoodPoints = 100;
    public float turnDelay = .1f;

    [HideInInspector]
    public bool playersTurn = true;

    private int level = 3;
    private List<Enemy> enemies;
    private bool enemiesMoving;

	// Use this for initialization
	void Awake () {
        if(instance == null) {
            instance = this;
        } else if(instance != this) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        boardScript = GetComponent<BoardManager>();
        enemies = new List<Enemy>();
        InitGame();
	}

    void InitGame() {
        enemies.Clear();
        boardScript.SetupScene(level);
    }
	
    public void GameOver() {
        enabled = false;
    }

	// Update is called once per frame
	void Update () {
        if(playersTurn || enemiesMoving) {
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