using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour {

    //sprite for open flag
    public Sprite flagOpen;
    //sprite for closed flag
    public Sprite flagClosed;

    //the level manager
    public LevelManager levelManager;
    
    //the sprite renderer for this object
    private SpriteRenderer spRender;
    //previous flag the player has touched
    private static GameObject previousFlag;

	// Use this for initialization
	void Start () {
        spRender = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if the collided object is a player
        if (collision.CompareTag("Player"))
        {
            //if there are already checkpoints reached previously and the current checkpoint is NOT the previous checkpoint
            if (previousFlag is GameObject && !(previousFlag.gameObject == this.gameObject))
            {
                //close the previous checkpoint
                previousFlag.GetComponent<CheckpointController>().spRender.sprite = flagClosed;
                //play sound effect
                levelManager.sm.PlaySound("flag01");

            } else if (previousFlag == null) {
                //play sound effect
                levelManager.sm.PlaySound("flag01");
            }
            //open the current checkpoint
            spRender.sprite = flagOpen;
            //set the previous flag to this checkpoint
            previousFlag = this.gameObject;
        }
    }
}
