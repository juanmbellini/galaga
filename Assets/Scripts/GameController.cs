using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameController : MonoBehaviour
{

	public GUIText scoreText;
	public int score;
	public GUIText livesText;
	public int livesRemaining;
	public List<Enemy> enemyPool = new List<Enemy>();

	public Enemy enemyPrefab;


// Use this for initialization
	void Start()
	{
		Physics2D.IgnoreLayerCollision(0, 9);
		Physics2D.IgnoreLayerCollision(9, 9);
		livesRemaining = 3;
		score = 0;
		//UpdateScore();
		for (int i = 0; i < 30; i++)
		{
			enemyPool.Add(Instantiate(enemyPrefab, new Vector3(10000, 0, 0), Quaternion.identity));
		}
}
	
	// Update is called once per frame
	void Update () {
		StartCoroutine(Spawn5Enemy());
	}


	IEnumerator Spawn5Enemy()
	{
		print(Time.time);
		yield return new WaitForSeconds(5);
		for (int i = 0; i < 5; i++)
		{
			enemyPool[i].Spawn(new Vector3(-30, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 10, 0), new Vector3(i*5-10, 10, 0));
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
