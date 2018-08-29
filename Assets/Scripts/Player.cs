using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class Player : MonoBehaviour {
    public float speed = 10.0f;
    public Rigidbody2D player;
    private float minPosX = -22.0f;
    private float maxPosX = 22.0f;
    public Animator animatorCtrl;
    private bool dead;
    private System.DateTime lastShootTime;
   

    public GameObject ShotPrefab;

    // Use this for initialization
    private void Start() {
        player.GetComponent<Rigidbody2D>();
        dead = false;
    }

    private void FixedUpdate() {
        CheckMove();
        CheckShoot();
    }

    private void CheckMove() {
        float newPosX = 0f;

        if (!dead)
        {

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                // Nueva posición de X
                newPosX = transform.position.x - speed * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                // Nueva posición de X
                newPosX = transform.position.x + speed * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
            {
                // Clampeo la posición a la pos mínima y máxima
                newPosX = Mathf.Clamp(newPosX, minPosX, maxPosX);

                // Asigno la posición
                transform.position = new Vector3(newPosX, transform.position.y, transform.position.z);
            }
        }
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
    
    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(col.gameObject.name + " : " + gameObject.name + " : " + Time.time);
        //Destroy(gameObject);
        animatorCtrl.SetTrigger("Death");
        dead = true;
    }
}