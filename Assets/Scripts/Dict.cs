using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Dict : SerializedMonoBehaviour {

    [SerializeField]
    public Dictionary<string, AudioClip> soundEffects;
    public Dictionary<string, GameObject> particleEffects;
    public List<Sprite> flashEffects;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
