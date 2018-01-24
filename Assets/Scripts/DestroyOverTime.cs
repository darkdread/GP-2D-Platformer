using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOverTime : MonoBehaviour {

    public float lifeTime;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //Countdown the lifetime
        lifeTime -= Time.deltaTime;

        //if the lifetime reaches 0 or below
        if (lifeTime <= 0) {
            //destroy the object
            Destroy(this.gameObject);
        }
	}
}
