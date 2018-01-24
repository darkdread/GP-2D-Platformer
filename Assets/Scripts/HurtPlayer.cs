using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtPlayer : MonoBehaviour {

    private LevelManager levelManager;
    public float damage;

	// Use this for initialization
	void Start () {
        levelManager = FindObjectOfType<LevelManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            //gets the player from level manager
            PlayerController player = levelManager.player;

            //if the player touches an Enemy
            if (CompareTag("Enemy")) {
                //if the player is falling and his y position is greater than the enemy's y position
                //print(Mathf.Abs(player.transform.position.y - transform.position.y));
                if (player.rb.velocity.y < 0 && Mathf.Abs(player.transform.position.y - transform.position.y) > 0.8) {
                    //makes the player jump again
                    player.Jump();
                    //"Destroy" the enemy
                    gameObject.SetActive(false);
                    //play the death sound effect
                    levelManager.sm.PlaySound("death01");
                    //show death animation
                    Instantiate(levelManager.Dict.particleEffects["death01"], transform.position, transform.rotation);

                    //stop the function from moving onwards
                    return;
                }
            }

            //knocks the player back and damages the player
            player.Knockback();
            player.DamagePlayer(damage);
        }
    }
}
