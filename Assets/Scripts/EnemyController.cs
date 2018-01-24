using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    //to check if the enemy is on the ground
    public Transform groundCheckLeft;
    public Transform groundCheckRight;
    public float groundCheckRadius;
    public LayerMask realGround;
    public bool isGrounded;

    //the enemy's movement speed
    public float speed;

    //timer to check if the enemy is on ground
    private float checkTimer;
    public static float checkTimerInitial;

    //if the enemy is moving to the right
    private bool moveRight;
    private Rigidbody2D rb;

    //initialize the delay
    EnemyController() {
        checkTimerInitial = 0.5f;
    }

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        checkTimer = checkTimerInitial;
	}
	
	// Update is called once per frame
	void Update () {

        //check if there are any ground on the left and right ground checker
        bool isGroundedLeft = Physics2D.OverlapCircle(groundCheckLeft.position, groundCheckRadius, realGround);
        bool isGroundedRight = Physics2D.OverlapCircle(groundCheckRight.position, groundCheckRadius, realGround);

        //decrease the timer to check
        checkTimer -= Time.deltaTime;

        //if any of the ground checker is false, that means the enemy is not on the ground
        isGrounded = isGroundedLeft && isGroundedRight;

        //if the enemy is not on the ground and the check timer is ready to check again
        if (!isGrounded && checkTimer <= 0) {
            //moves the enemy to the left
            moveRight = !moveRight;
            //reset the check timer
            checkTimer = checkTimerInitial;
        }

        //if the enemy is moving right, the velocity is positive. Vice-versa
        float _speed = moveRight ? speed : -speed;
        //if the enemy is moving right, the scale x is positive. Vice-versa
        float _scale = moveRight ? 1 : -1;

        //adjust velocity of enemy accordingly
        rb.velocity = new Vector2(_speed, rb.velocity.y);
        //adjust scale x of enemy accordingly
        transform.localScale = new Vector3(_scale, transform.localScale.y, transform.localScale.z);

	}
}
