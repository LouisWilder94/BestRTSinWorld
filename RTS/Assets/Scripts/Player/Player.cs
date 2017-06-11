using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class Player : NetworkBehaviour {
    public Camera playerCamera;
    public int Minerals;


    [HideInInspector]
    public int playerNumber = 10000;
    [HideInInspector]
    public string randomID;
    [HideInInspector]
    public long timeOnStartup;

    

    public Vector3 mousePosition
    {
        get
        {
            Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(inputRay, out hit))
            {
                return hit.point;
            }
            else
            {
                Debug.Log("Mouse Not Hitting Something 8[]D~~~~~");
                return Vector3.up;
            }
        }
    }

    public Vector3 camPosition
    {
        get
        {
            return playerCamera.transform.position;
        }
        set
        {
            playerCamera.transform.position = value;
        }
    }


    [ServerCallback]
    private void Awake()
    {
        Debug.Log("Adding Player" + GameManager.numPlayers);
        timeOnStartup = DateTime.Now.Ticks;
        randomID = Guid.NewGuid().ToString(); //Random.Range(0, 100f);
        GameManager.instance.RpcAddPlayerToList(this);
        GameManager.IncreasePlayerNumber();
    }
}
