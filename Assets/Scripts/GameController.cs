﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Random = System.Random;

public class GameController : MonoBehaviour
{

	public GUIText scoreText;
	public int score;
	public GUIText livesText;
	public int livesRemaining;
	public int enemiesAlive;
	public List<Enemy> enemyPool = new List<Enemy>();

	public Enemy enemyPrefab;


// Use this for initialization
	void Start()
	{
		Physics2D.IgnoreLayerCollision(9, 9);
		livesRemaining = 3;
		enemiesAlive = 0;
		score = 0;
		//UpdateScore();
		for (int i = 0; i < 10; i++)
		{
			enemyPool.Add(Instantiate(enemyPrefab, new Vector3(10000, 0, 0), Quaternion.identity));
		}
		StartCoroutine(SpawnEnemies());
}
	
	// Update is called once per frame
	void Update () {
		StartCoroutine(SpawnEnemies());
	}

	
	IEnumerator SpawnEnemiesStart()
	{
		yield return new WaitForSeconds(8);
		int i = 0;
		foreach (Enemy e in enemyPool)
		{
			if (!e.IsAlive())
			{
				e.Spawn(new Vector3(-30, 0, 0), new Vector3(UnityEngine.Random.Range(-5,5), UnityEngine.Random.Range(-5,5), 0), new Vector3(0, 10, 0), new Vector3(i*5-22, 2*enemiesAlive, 0));
				i++;
				enemiesAlive++;
			}
		}
	}

	IEnumerator SpawnEnemies()
	{
		yield return new WaitForSeconds(8);
		int i = 0;
		foreach (Enemy e in enemyPool)
			{
				if (!e.IsAlive())
				{
					e.Spawn(new Vector3(-30, 0, 0), new Vector3(UnityEngine.Random.Range(-5,5), UnityEngine.Random.Range(-5,5), 0), new Vector3(0, 10, 0), new Vector3(UnityEngine.Random.Range(-22,22), 2*enemiesAlive, 0));
					i++;
					enemiesAlive++;
				}
			}
	}
	
	public void AddScore(int newScoreValue)
	{	
		score += newScoreValue;
		UpdateScore();
	}
	
	void UpdateScore()
	{
		scoreText.text = "Score: " + scoreText;
	}

	void instantiate()
	{
		Debug.Log("ADSA");
	}
}
