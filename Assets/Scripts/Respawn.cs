using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour {

    [SerializeField]
    float x = 0f;
    [SerializeField]
    float y = 5f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player") {
            collision.gameObject.transform.position = new Vector2(x,y);
        } else if (collision.gameObject.tag == "Flame") {
            Destroy(collision.gameObject);
        }
    }
}
