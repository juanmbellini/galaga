using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

	public float moveSpeed = 10.0f;
	public Rigidbody2D player;
	
	// Use this for initialization
	void Start ()
	{
		player.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate()
	{
		MovePlayer();
	}

	public void MovePlayer()
	{
		player.velocity = new Vector2(Input.GetAxis("Horizontal"), 0.0f) * moveSpeed;
	}
}
