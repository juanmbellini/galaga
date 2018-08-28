using UnityEngine;

public class Shot : MonoBehaviour {
    public Rigidbody2D body;
    public BoxCollider2D collider;
    public float speed = 50.0f;


    private void Start() {
        body.velocity = new Vector2(0, speed);
    }

    private void Update() {
     
    }
    
    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(col.gameObject.name + " : " + gameObject.name + " : " + Time.time);
        Destroy(gameObject);
    }
}