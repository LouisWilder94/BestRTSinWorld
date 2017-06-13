using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CursorColor : NetworkBehaviour {
    public ParticleSystem particles;
    public Light cursorLight;

    [SyncVar]
    Color playercolor;
	// Use this for initialization
	void Start () {
        particles = GetComponent<ParticleSystem>();
        cursorLight = GetComponent<Light>();

        playercolor = GameManager.instance.playerColors[GetComponentInParent<Player>().playerNumber];
        particles.startColor = playercolor;
        cursorLight.color = playercolor;
	}

}
