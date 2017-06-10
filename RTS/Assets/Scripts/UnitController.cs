﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UnitController : NetworkBehaviour {

    public GameObject meleePrefab;
    public GameObject rangedPrefab;

    string myTag;

    GameObject[] myUnits;

    [SerializeField]
    int maxUnits = 25;

	// Use this for initialization
	void Start ()
    {
        if(GameObject.FindGameObjectsWithTag("Player1").Length == 0)
        {
            myTag = "Player1";
        }else
        {
            myTag = "Player2";
        }

        gameObject.tag = myTag;

        myUnits = new GameObject[25];

        for(int i = 0; i < maxUnits; i++)
        {
            myUnits[i] = meleePrefab;
        }
	}
	
	void Update ()
    {
        if (!isLocalPlayer)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                CmdMeleeUnitSpawn(hit.point, Quaternion.identity);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                CmdRangedUnitSpawn(hit.point, Quaternion.identity);
            }
        }
	}


    [Command]
    void CmdMeleeUnitSpawn( Vector3 hitLocation, Quaternion rotation)
    {
        Debug.Log("Create Melee.");

        GameObject instance = Instantiate(meleePrefab, hitLocation, rotation) as GameObject;
        instance.tag = myTag;

        NetworkServer.Spawn(instance);
    }

    [Command]
    void CmdRangedUnitSpawn(Vector3 hitLocation, Quaternion rotation)
    {
        Debug.Log("Create Ranged.");

        GameObject instance = Instantiate(rangedPrefab, hitLocation, rotation) as GameObject;
        instance.tag = myTag;

        NetworkServer.Spawn(instance);
    }
}
