using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    Rigidbody2D rigidbody;
    Animator animator;
    CircleCollider2D collider;
    public GameObject rainbow;
    public GameObject dash;
    public GameObject shit;
    public GameObject flame;
    public LayerMask terrainLayer;

    float lateralInput = 0;
    float verticalInput = 0;
    float dashInput = 0;
    float shitInput = 0;
    float flameInput = 0;
    float rainbowInput = 0;
    long dashCooldownTimer = 0;

    [SerializeField]
    bool isGrounded = false;
    bool hasJumped = true;
    bool hasAirJumped = true;
    bool hasDownJumped = true;
    bool hasDashed = true;
    bool isDashing = false;
    bool jumpPressed = true;
    bool flamePressed = true;

    bool rainbowActivated = false;
    bool rainbowPressed = false;

    [SerializeField]
    float groundDistance = 0.2f;
    [SerializeField]
    float gravityScale = 3f;
    [SerializeField]
    float moveSpeed = 20f;
    [SerializeField]
    float moveForce = 20f;
    [SerializeField]
    float maxVelocity = 20f;
    [SerializeField]
    float jumpForce = 25f;
    [SerializeField]
    float airJumpForce = 20;
    [SerializeField]
    float downJumpForce = 25f;
    [SerializeField]
    float dashForce = 200f;
    [SerializeField]
    float dashTime = 0.1f;
    [SerializeField]
    float dashCooldown = 0.5f;
    [SerializeField]
    float flameForce = 50f;
    [SerializeField]
    float flameAngle = 0.3f;
    [SerializeField]
    int maxFlames = 3;
    [SerializeField]
    int rainbowEveryFrame = 1;
   

    // Use this for initialization
    void Start () {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        collider = GetComponent<CircleCollider2D>();

        rigidbody.gravityScale = gravityScale;

        Physics2D.IgnoreLayerCollision(8, 10, true);    // Ignore collision between Shit(8) and Player(10)
        Physics2D.IgnoreLayerCollision(9, 10, true);    // Ignore collision between Flame(9) and Player(10)
        Physics2D.IgnoreLayerCollision(9, 9, true);      // Ignore collision between Flame(9) and Flame(9)
    }
	
	// Update is called once per frame
	void Update () {
        GetInput();
        Move();
        Jump();
        Shit();
        Flame();
        Rainbow();
        CheckGrounded();
        Respawn();

        dashCooldownTimer++;
	}

    private void GetInput() {
        lateralInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Jump");
        dashInput = Input.GetAxis("Dash");
        shitInput = Input.GetAxis("Shit");
        flameInput = Input.GetAxis("Flame");
        rainbowInput = Input.GetAxis("Rainbow");
    }

    private void Move() {
        if (!hasDownJumped) {

            rigidbody.velocity = new Vector2(lateralInput * moveSpeed,rigidbody.velocity.y);
            
            // For Shit Explosion
            /*if (lateralInput > 0 && rigidbody.velocity.x < maxVelocity) {
                rigidbody.AddForce(Vector2.right * lateralInput * moveForce,ForceMode2D.Force);
            } else if (lateralInput < 0 && rigidbody.velocity.x > -maxVelocity) {
                rigidbody.AddForce(Vector2.right * lateralInput * moveForce,ForceMode2D.Force);
            } else if(lateralInput == 0 && isGrounded) {
                rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
            }*/
        }

        if(rigidbody.velocity.x > 0) {
            rigidbody.transform.localScale = new Vector3(3,3,1);
        } else if (rigidbody.velocity.x < 0) {
            rigidbody.transform.localScale = new Vector3(-3,3,1);
        }

        animator.SetFloat("isMoving",Mathf.Abs(lateralInput));
    }

    void Jump() {
        if (!isDashing) {
            if (verticalInput > 0 && isGrounded && !hasJumped && !jumpPressed) {
                rigidbody.AddForce(Vector2.up * verticalInput * jumpForce,ForceMode2D.Impulse);
                hasJumped = true;
                jumpPressed = true;
            } else if (verticalInput > 0 && !isGrounded && !hasAirJumped && !jumpPressed) {
                if (rigidbody.velocity.y < 0) {
                    rigidbody.velocity = new Vector2(rigidbody.velocity.x,0);
                }
                rigidbody.AddForce(Vector2.up * verticalInput * airJumpForce,ForceMode2D.Impulse);
                hasAirJumped = true;
                hasDownJumped = false;
                jumpPressed = true;
            } else if (dashInput > 0 && lateralInput != 0 && !hasDashed && dashCooldownTimer * Time.deltaTime > dashCooldown) {
                rigidbody.gravityScale = 0f;
                rigidbody.velocity = Vector2.zero;
                rigidbody.AddForce(new Vector2(dashForce * lateralInput, 0), ForceMode2D.Impulse);

                isDashing = true;
                if (!isGrounded) {
                    hasDashed = true;
                } else {
                    hasDashed = false;
                }
                hasDownJumped = false;
                dashCooldownTimer = 0;

                Instantiate(dash,rigidbody.position,Quaternion.identity);
            } else if (verticalInput < 0 && !isGrounded && !hasDownJumped && !jumpPressed) {
                rigidbody.velocity = Vector2.zero;
                rigidbody.AddForce(Vector2.up * verticalInput * downJumpForce,ForceMode2D.Impulse);
                hasDownJumped = true;
            }
        } else if (isDashing) {
            if(dashCooldownTimer * Time.deltaTime > dashTime) {
                rigidbody.velocity = Vector2.zero;
                rigidbody.gravityScale = gravityScale;
                isDashing = false;
            }
        }

        if (verticalInput == 0) {
            jumpPressed = false;
        }

        animator.SetBool("isJumping",!isGrounded);
        animator.SetBool("isAirJumping",hasAirJumped);
        animator.SetBool("isDownJumping",hasDownJumped);
        animator.SetBool("isDashing",isDashing);
    }

    void Shit() {
        if (shitInput > 0 && GameObject.FindGameObjectWithTag("Shit") == null) {
            Instantiate(shit,rigidbody.position,Quaternion.identity);
        }
    }

    void Flame() {
        if (flameInput > 0 && !flamePressed && GameObject.FindGameObjectsWithTag("Flame").Length < maxFlames) {
            GameObject f = Instantiate(flame,rigidbody.position,Quaternion.identity);
            Rigidbody2D rF = f.GetComponent<Rigidbody2D>();
            rF.velocity = rigidbody.velocity;
            rF.AddForce(new Vector2((1 - flameAngle) * flameForce * Mathf.Sign(rigidbody.transform.localScale.x), flameAngle * flameForce),ForceMode2D.Impulse);

            flamePressed = true;
        } else if (flameInput == 0) {
            flamePressed = false;
        }
    }

    void Rainbow() {
        if (rainbowInput > 0 && !rainbowPressed) {
            rainbowActivated = !rainbowActivated;
            rainbowPressed = true;
        } else if(rainbowInput == 0) {
            rainbowPressed = false;
        }

        if (rainbowActivated) {
            Instantiate(rainbow, rigidbody.position, Quaternion.identity);
        }
    }

    void Respawn() {
        if(rigidbody.position.y < -20) {
            rigidbody.velocity = Vector2.zero;
            rigidbody.position = new Vector2(0, 5);
        }
    }

    void CheckGrounded() {
        float raycastOffset = collider.radius;
        if (Physics2D.Raycast(new Vector2(transform.position.x + 1f, transform.position.y), Vector2.down, groundDistance, terrainLayer)
            || Physics2D.Raycast(new Vector2(transform.position.x - 1f,transform.position.y),Vector2.down,groundDistance,terrainLayer)) {
            isGrounded = true;
            hasJumped = false;
            hasAirJumped = false;
            hasDownJumped = false;
            hasDashed = false;
        } else {
            isGrounded = false;
        }
    }

    /*void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.tag == "Terrain") {
            isGrounded = true;
            hasJumped = false;
            hasAirJumped = false;
            hasDownJumped = false;
            hasDashed = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if(collision.gameObject.tag == "Terrain") {
            isGrounded = false;
        }
    }*/
}
