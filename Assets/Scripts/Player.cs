using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public float moveSpeed = 10.0f;
    public Rigidbody2D player;
    private System.DateTime lastShootTime;
   

    public GameObject ShotPrefab;

    // Use this for initialization
    private void Start() {
        player.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        CheckMove();
        CheckShoot();
    }

    private void CheckMove() {
        player.velocity = new Vector2(Input.GetAxis("Horizontal"), 0.0f) * moveSpeed;
    }
    

    private void CheckShoot() {        
        if (!Input.GetKeyDown(KeyCode.Space)) {
            return;
        }
        var ts = System.DateTime.Now - lastShootTime;
        if (!(ts.TotalMilliseconds > 300.0f)) return;
        lastShootTime = System.DateTime.Now;
        var shot = Instantiate(ShotPrefab);
        shot.transform.position = new Vector3(transform.position.x, transform.position.y + 1.0f, transform.position.z);

    }
}