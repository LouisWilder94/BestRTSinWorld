using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSound : MonoBehaviour {
    public AudioSource source;

    public AudioClip[] clips;

	// Use this for initialization
	void Start ()
    {
        source.clip = clips[Random.Range(0, clips.Length)];
        source.PlayOneShot((clips[Random.Range(0, clips.Length)]), 1f);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
