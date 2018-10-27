using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShitBehaviour : MonoBehaviour {

    Rigidbody2D rigidbody;
    Animator animator;
    public GameObject fireObject;

    bool isActive = false;
    bool isExploding = false;
    bool hasExploded = false;
    GameObject fire;

    [SerializeField]
    float fuseTime = 3f;
    [SerializeField]
    float explosionForce = 50f;
    [SerializeField]
    float explosionRange = 20f;
    [SerializeField]
    float duration = 10f;

    long time = 0;

	// Use this for initialization
	void Start () {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        Active();
        Explode();
        FirePosition();
        DestroyIfUseless();

        time++;
        animator.SetBool("isExploding",isExploding);
    }

    void Active() {
        if(isActive && time * Time.deltaTime > fuseTime) {
            isExploding = true;
            gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
        }
    }

    void Explode() {
        animator.GetCurrentAnimatorStateInfo(0);

        if (isExploding) {
            AnimatorStateInfo animation = animator.GetCurrentAnimatorStateInfo(0);

            if (animation.IsName("ShitExploding") && animation.normalizedTime > 0.5 && !hasExploded) {
                Destroy(fire);

                GameObject player = GameObject.FindGameObjectWithTag("Player");
                
                // For Shit Explosion With Player
                /*float distance = Vector3.Distance(gameObject.transform.position,player.transform.position);
                if (distance < explosionRange) {
                    Rigidbody2D pRB = player.GetComponent<Rigidbody2D>();

                    float xForce = (pRB.position.x - rigidbody.position.x) * (1 - (distance / explosionRange)) * explosionForce;
                    float yForce = (pRB.position.y - rigidbody.position.y) * (1 - (distance / explosionRange)) * explosionForce;

                    pRB.AddForce(new Vector2(xForce, yForce), ForceMode2D.Impulse);
                }*/

                GameObject[] buildings = GameObject.FindGameObjectsWithTag("Building");
                foreach (GameObject go in buildings) {
                    if (Vector3.Distance(gameObject.transform.position, go.transform.position) < explosionRange) {
                        go.GetComponent<BuildingBehaviour>().isExploding = true;
                    }
                }

                hasExploded = true;
            } else if (animation.IsName("ShitExploding") && animation.normalizedTime > 1 && hasExploded) {
                Destroy(gameObject);
            }
        }
    }

    void FirePosition() {
        if(fire != null) {
            fire.transform.position = rigidbody.position;
            fire.transform.rotation = gameObject.transform.rotation;
        }
    }

    void DestroyIfUseless() {
        if (time * Time.deltaTime > duration && !isActive) {
            Destroy(gameObject);
        }

        if (rigidbody != null) {
            if (rigidbody.position.y < -20) {
                Destroy(gameObject);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Flame" && !isActive) {
            fire = Instantiate(fireObject, rigidbody.position, Quaternion.identity);
            
            isActive = true;
            time = 0;
        }

        rigidbody.velocity = Vector2.zero;
    }
}
