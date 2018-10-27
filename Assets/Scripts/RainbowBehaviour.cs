using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainbowBehaviour : MonoBehaviour {

    SpriteRenderer image;

    float time = 0;

    [SerializeField]
    float timeTrasparencyStart = 3f;
    [SerializeField]
    float timeTrasparencyDuration = 1.5f;

	// Use this for initialization
	void Start () {
        image = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        float currentTime = time * Time.deltaTime;

        if (currentTime > timeTrasparencyStart) {
            float trasparency = Mathf.Max((1 - (currentTime - timeTrasparencyStart) / (timeTrasparencyDuration)), 0);
            image.color = new Color(1f, 1f, 1f, trasparency);
        }

        if(currentTime > timeTrasparencyStart + timeTrasparencyDuration) {
            Destroy(gameObject);
        }

        time++;
	}
}
