using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitColor : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if(gameObject.tag == "Player1")
        {
            gameObject.GetComponent<Renderer>().material.color = Color.blue;
        }
        else
        {
            gameObject.GetComponent<Renderer>().material.color = Color.red;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
