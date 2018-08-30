using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Object = System.Object;
using Random = System.Random;

public class GameController : MonoBehaviour
{
	

	public TextMeshProUGUI scoreText;
	public TextMeshProUGUI livesText;
	public int enemiesAlive;
	public List<Enemy> enemyPool = new List<Enemy>();
	public ScoreController _scoreController;
	public static GameController Instance;
	public Player _player;
	public Enemy enemyPrefab;

	private void Awake()
	{
		_scoreController = FindObjectOfType<ScoreController>();

		if (GameController.Instance == null)
		{
			GameController.Instance = this;
			DontDestroyOnLoad(this.scoreText);
			DontDestroyOnLoad(this.livesText);
			DontDestroyOnLoad(_scoreController);
		}
		else
		{
			Destroy(this);
		}
	}

// Use this for initialization
	void Start()
	{
		_player = FindObjectOfType<Player>();
		Physics2D.IgnoreLayerCollision(9, 9);
		Physics2D.IgnoreLayerCollision(10, 10);
		enemiesAlive = 0;
		for (int i = 0; i < 10; i++)
		{
			enemyPool.Add(Instantiate(enemyPrefab, new Vector3(10000, 0, 0), Quaternion.identity));
		}
		UpdateUI();
		StartCoroutine(SpawnEnemies());
}
	
	// Update is called once per frame
	void Update () {
		UpdateUI();
		StartCoroutine(SpawnEnemies());
	}

	public void PlayerDeath()
	{
		StartCoroutine(WaitDeath());
		loseLife();
		if(_scoreController.livesRemaining != 0)SceneManager.LoadScene("MainScene");
		else
		{
			Destroy(_scoreController);
			SceneManager.LoadScene("GameOver");
		}
	}

	IEnumerator WaitDeath()
	{
		yield return new WaitForSeconds(2);
	}
	
	
	IEnumerator SpawnEnemiesStart()
	{
		yield return new  WaitForSeconds(5);
		int i = 0;
		foreach (Enemy e in enemyPool)
		{
			if (!e.IsAlive())
			{
				int coin = UnityEngine.Random.Range(-1, 1);
				e.Spawn(new Vector3(coin<0 ? 30:-30, 0, 0), new Vector3(UnityEngine.Random.Range(-5,5), UnityEngine.Random.Range(-5,5), 0), new Vector3(0, 10, 0), new Vector3(i*5-22, 2*enemiesAlive, 0));
				i++;
				enemiesAlive++;
			}
		}
	}

	IEnumerator SpawnEnemies()
	{
		yield return new WaitForSeconds(3);
		int i = 0;
		foreach (Enemy e in enemyPool)
			{
				if (!e.IsAlive())
				{
					int coin = UnityEngine.Random.Range(-1, 1);
					e.Spawn(new Vector3(coin<0 ? 30:-30, 0, 0), new Vector3(UnityEngine.Random.Range(-5,5), UnityEngine.Random.Range(-5,5), 0), new Vector3(0, 10, 0), new Vector3(UnityEngine.Random.Range(-22,22), 2*enemiesAlive, 0));
					i++;
					enemiesAlive++;
				}
			}
	}
	
	public void AddScore(int newScoreValue)
	{	
		_scoreController.score += newScoreValue;
		UpdateUI();
	}
	
	public void loseLife()
	{	
		_scoreController.livesRemaining--;
		UpdateUI();
	}
	
	void UpdateUI()
	{
		scoreText.text = "Score: " + _scoreController.score;
		String livesImg = "";
		for (int i = 0; i < _scoreController.livesRemaining; i++)
		{
			livesImg = livesImg + "X";
		}
		livesText.text = "Lives: " + livesImg;
	}

	
}
