using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBehaviour : MonoBehaviour {

    Animator animator;

    public bool isExploding = false;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        animator.SetBool("isExploding",isExploding);

        if (isExploding) {
            AnimatorStateInfo animation = animator.GetCurrentAnimatorStateInfo(0);

            if (animation.IsName("BuildingExplosion") && animation.normalizedTime > 1) {
                Destroy(GetComponent<BoxCollider2D>());
            }
        }

        
	}
}
