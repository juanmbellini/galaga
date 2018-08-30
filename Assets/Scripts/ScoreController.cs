using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour
{

	public int score;
	public int livesRemaining;
	
	// Use this for initialization
	void Start () {
		livesRemaining = 3;
		score = 0;
	}
	
	// Update is called once per frame
	void Update () {
	}
}
