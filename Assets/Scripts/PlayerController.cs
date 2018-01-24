using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private const float defaultJumpSpeed = 8f;

    //the movement speed of the player
    public float moveSpeed;
    //the jumping speed of the player
    public float jumpSpeed;
    // The score of the player
    public float score;

    //to check if the player is on the ground
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask realGround;
    public bool isGrounded;

    //the respawn position of the player
    public Vector3 respawnPosition;
    //the health & max health of the player
    public float health;
    public float maxHealth;

    //knockback force
    public float knockbackForce;
    //the amount of time to get knockbacked
    public float knockbackLength;
    //the amount of time to trigger knockback on player again
    private float knockbackTimer;

    //to animate the player
    private Animator myAnim;
    //to apply physics for the player
    public Rigidbody2D rb;
    //the level manager
    private LevelManager levelManager;

    // Bullet
    public GameObject bulletPrefab;
    public float fireRate;
    private float nextFire;
    

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
        levelManager = FindObjectOfType<LevelManager>();

        //player's initial health
        health = maxHealth;
        //player's initial respawn position
        respawnPosition = this.gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        //get player's input on the horizontal axis.
        var x = Input.GetAxisRaw("Horizontal");
        //check if the player is on the ground.
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, realGround);

        //if the player isn't getting knocked backed
        if (knockbackTimer <= 0) {
            //if the player is moving left/right
            if (x != 0) {
                //if the player is moving to the right, set the movement speed to positive. Vice-versa.
                x = (x > 0) ? moveSpeed : -moveSpeed;
                //if the player is moving to the right, set the scale to positive. Vice-versa.
                float scaleX = (x > 0) ? 1 : -1;
                //scale the player accordingly.
                transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);
                //move the player according to the movement speed.
                rb.velocity = new Vector2(x, rb.velocity.y);
            } else {
                //if the player isn't moving, set the velocity to 0.
                rb.velocity = new Vector2(0, rb.velocity.y);
            }

            //if the player presses the jump key and is on the ground
            if (Input.GetButtonDown("Jump") && isGrounded) {
                //the player jumps according to the jump speed.
                rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            }

            if (Input.GetKeyDown(KeyCode.L) && Time.time > nextFire) {
                nextFire = Time.time + fireRate;
                Vector2 vel = new Vector2(transform.localScale.x * 5f, 0);
                Vector2 bulletPos = new Vector2(transform.position.x + transform.localScale.x * 1f, transform.position.y - 0.43f);
                GameObject bullet = Instantiate<GameObject>(bulletPrefab, bulletPos, Quaternion.identity);
                bullet.transform.localScale = new Vector3(bullet.transform.localScale.x * transform.localScale.x, bullet.transform.localScale.y, bullet.transform.localScale.z);
                bullet.GetComponent<Rigidbody2D>().velocity = vel;
                Destroy(bullet, 5f);
            }
        } else if (knockbackTimer > 0) {
            //if the player is getting knockedbacked, decrease the timer
            knockbackTimer -= Time.deltaTime;
        }

        //setting variables for animator so the animator knows what state to animate
        myAnim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        myAnim.SetBool("Ground", isGrounded);
    }

    //allows other classes to make the player jump
    public void Jump(float speed = defaultJumpSpeed) {
        rb.velocity = new Vector2(rb.velocity.x, speed);
    }

    //allows other classes to make the player get knockedbacked
    public void Knockback() {

        //if the player is not already getting knocked
        if (knockbackTimer <= 0) {
            //set the length of the knockback
            knockbackTimer = knockbackLength;

            //set the force, if the player is facing right, knock him to the left. Vice-versa
            float _knockbackForce = transform.localScale.x > 0 ? -knockbackForce : knockbackForce;
            rb.velocity = new Vector2(_knockbackForce, Mathf.Abs(_knockbackForce));
        }
    }

    //allows other classes to damage the player
    public void DamagePlayer(float value) {
        
        //decrease player's health
        health -= value;

        // If health is less than 0, set it to 0. If health is greater than max health, set it to max health. Otherwise, leave it.
        health = (health < 0) ? 0 : (health > maxHealth) ? maxHealth : health;

        //update player's healthbar
        levelManager.UpdateHealth();

        // Player is damaged, play hit sound effect.
        if (value > 0) {
            //play sound effect
            List<string> sfxArr = new List<string>() { "player_attacked_01", "player_attacked_02" };
            levelManager.sm.PlayRandomSound(sfxArr);

            //play flashing effect
            levelManager.FadeUI(0.3f);
        } else {
            // Player is healed, play healed sound effect.
            levelManager.sm.PlaySound("player_coin01");
        }

        //if the player's health is 0, kill and respawn the player
        if (health <= 0) {
            levelManager.Respawn();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if the collided object is a kill plane
        if (collision.name == "KillPlane")
        {
            //damage the player (instantly kills her)
            this.DamagePlayer(this.health);
        }
        //if the collided object is a checkpoint (Flag)
        else if (collision.CompareTag("Flag"))
        {
            //set the respawn position to the flag the player collided with
            respawnPosition = collision.transform.position;
        } else if (collision.CompareTag("Coin")) {
            // Increase the score by 1
            score++;

            // Update the UI
            levelManager.UpdateScore();

            // Player coin pickup sound
            levelManager.sm.PlaySound("player_coin01");

            // Destroy the coin
            Destroy(collision.gameObject);
        } else if (collision.CompareTag("Health")) {
            // Heal player for 200
            this.DamagePlayer(-200);

            // Destroy the coin
            Destroy(collision.gameObject);
        }

    }
}
