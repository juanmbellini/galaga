using UnityEngine;

public class Shot : MonoBehaviour {
    public Rigidbody2D body;
    public float speed = 20.0f;


    private void Start() {
        body.velocity = new Vector2(0, speed);
    }

    private void Update() {
        // Do nothing
    }
}