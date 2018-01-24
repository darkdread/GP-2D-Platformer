using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeeingEnemy : MonoBehaviour {

    public SpriteRenderer sr;
    public LayerMask WhoICanSee;

	// Use this for initialization
	void Start () {
        sr = GetComponent<SpriteRenderer>();
	}

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            Vector2 dir = collision.transform.position - transform.position;
            float dist = Vector2.Distance(transform.position, collision.transform.position);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, dist, WhoICanSee);

            if (hit.collider == collision) {
                sr.color = new Color32(255, 180, 180, 255);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            sr.color = new Color32(255, 255, 255, 255);
        }
    }
}
