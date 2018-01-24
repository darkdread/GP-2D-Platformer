using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sys = System.Diagnostics;


public class SoundManager : MonoBehaviour {

    //where to play the sound
    public GameObject target;

    //dictionary contains all the sound effects
    private Dict sd;
    //for easier reference
    private Dictionary<string, AudioClip> s;

	// Use this for initialization
	void Start () {
        //get the sound dictionary
        sd = this.GetComponent<Dict>();
        //set the dictionary for easier reference
        s = sd.soundEffects;
	}
	
	// Update is called once per frame
	void Update () {

    }

    public void PlayRandomSound(List<string> soundList, float _volume = 1, float _pitch = 1) {

        //loop through the list of sounds given
        IEnumerator<string> enumerator = soundList.GetEnumerator();
        //if all of them are in the sound dictionary
        if (VerifySoundList(enumerator)) {
            //randomly get a sound from the list given
            string sound = soundList[(int) Mathf.Floor(UnityEngine.Random.Range(0, soundList.Count))];
            //play the sound
            PlaySound(sound, _volume, _pitch);
        }
    }

    //make sure the parsed sound list is in the sound dictionary
    private bool VerifySoundList(IEnumerator<string> enumerator) {
        //iterate through the next item if there is any
        while (enumerator.MoveNext()) {
            //if the sound dictionary does not contain the item
            if (!s.ContainsKey(enumerator.Current)) {
                Debug.Log(String.Format("Error! {0} not found in Sound Dictionary.", enumerator.Current));
                return false;
            }
        }
        return true;
    }

    public void PlaySound(string sound, float _volume = 1, float _pitch = 1) {
        if (s.ContainsKey(sound)) {
            //create a dummy object to play the sound
            GameObject dummy = new GameObject("SFX: " + sound);
            //creating the sound
            AudioSource AS = dummy.AddComponent<AudioSource>();
            //adjust the properties of the sound
            AS.volume = _volume;
            AS.pitch = _pitch;
            AS.clip = s[sound];
            AS.PlayOneShot(AS.clip);

            //setting the position of the dummy
            dummy.transform.position = target.transform.position;
            //destroy the dummy after the sound has finished playing
            Destroy(dummy, AS.clip.length);
        }
    }
}
