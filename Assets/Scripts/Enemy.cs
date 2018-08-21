using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public Rigidbody2D body;

	// Use this for initialization
	private void Start () {
		
	}
	
	// Update is called once per frame
	private void Update () {		
	}

	private void OnCollisionEnter (Collision col)
	{
		Debug.Log("Choque");
		if(col.gameObject.tag == "shot")
		{
			Destroy(col.gameObject);
		}
	}
}
