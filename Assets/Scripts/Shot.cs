using UnityEngine;

public class Shot : MonoBehaviour {
    public Rigidbody2D body;
    public BoxCollider2D collider;
    public float speed = 30.0f;
    [SerializeField] private GameController _gameController;

    private void Start() {
        body.velocity = new Vector2(0, speed);
        _gameController = FindObjectOfType<GameController>();
    }

    private void Update() {
        if (transform.position.y >= 40.0f)
        {
            speed = 0;
            transform.position = new Vector3(-10000, 0, 0);
            speed = 30;
        }
    }
    
    void OnTriggerEnter2D(Collider2D col)
    {
         Debug.Log(col.gameObject.name + " : " + gameObject.name + " : " + Time.time);
        _gameController.AddScore(100);
        speed = 0;
        transform.position = new Vector3(-10000, 0, 0);
        speed = 30;
        //Destroy(gameObject);
    }
}