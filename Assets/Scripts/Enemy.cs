using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public Rigidbody2D body;
	public BoxCollider2D collider;

	// Use this for initialization
	private void Start () {
		
	}
	
	// Update is called once per frame
	private void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		Debug.Log(col.gameObject.name + " : " + gameObject.name + " : " + Time.time);
		Destroy(gameObject);
	}
}
