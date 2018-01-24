
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    //the variable which holds the player
    public GameObject player;
    //the amount of units to move ahead
    public float followAhead;
    //how long it will take for the camera to transition
    public float smoothing;

    //the position to move the camera
    private Vector3 targetPosition;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //if the player is facing right, move the camera to the right. Vice-versa
        float facingRight = (player.transform.localScale.x > 0) ? followAhead : -followAhead;
        //if the player is on the ground, set the camera y position to 0. Or else, follow the player
        float yPos = player.transform.position.y > 0.1? player.transform.position.y: 0;

        //the position to move the camera
        targetPosition = new Vector3(player.transform.position.x + facingRight, yPos, transform.position.z);

        //move the camera with some smoothing
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime);
	}
}
