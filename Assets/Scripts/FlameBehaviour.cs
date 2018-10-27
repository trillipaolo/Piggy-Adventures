using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameBehaviour : MonoBehaviour {

    Rigidbody2D rigidbody;
    Collider2D collider;

    [SerializeField]
    float duration = 10f;
    
    long time = 0;

    // Use this for initialization
    void Start() {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<CircleCollider2D>();
        
        rigidbody.transform.localScale = new Vector3(Mathf.Sign(rigidbody.velocity.x), 1, 1);
    }

    // Update is called once per frame
    void Update() {

        if (rigidbody != null) {
            gameObject.transform.rotation = new Quaternion(rigidbody.velocity.x, rigidbody.velocity.y,0,0);
        }

        if (time * Time.deltaTime > duration) {
            Destroy(gameObject);
        }

        if (gameObject.transform.position.y < -20) {
            Destroy(gameObject);
        }

        time++;
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Shit") {
            Destroy(gameObject);
        } else {
            if (rigidbody != null) {
                rigidbody.velocity = Vector2.zero;
            }
            Destroy(rigidbody);
        }
    }
}
