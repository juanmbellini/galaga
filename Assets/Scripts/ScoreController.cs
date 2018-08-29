using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour
{

	public TextMeshProUGUI score;
	public TextMeshProUGUI lives;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log(score.text);
		Debug.Log(lives.text);
	}
}
