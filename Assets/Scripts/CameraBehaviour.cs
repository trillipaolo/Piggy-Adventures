using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour {

    Transform player;
    public GameObject background;

    Vector3 velocity = Vector3.zero;
    
    [SerializeField]
    float smooth = 0.3f;
    [SerializeField]
    float xOffset = 0f;
    [SerializeField]
    float yOffset = 3f;
    [SerializeField]
    float zOffset = -10f;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
	}
	
	// Update is called once per frame
	void Update () {
		if(player != null) {
            Vector3 targetPosition = new Vector3(player.position.x + xOffset,player.position.y + yOffset,player.position.z + zOffset);
            transform.position = Vector3.SmoothDamp(transform.position,targetPosition,ref velocity,smooth);
        }

        if (background != null) {
            background.transform.position = new Vector3(gameObject.transform.position.x,gameObject.transform.position.y,0);
        }
    }
}
