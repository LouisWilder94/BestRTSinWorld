using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class Player : NetworkBehaviour {
    public Camera playerCamera;
    public int Minerals;
    public Color playerColor;

    public UnitController unitController;

    [HideInInspector]
    public int playerNumber = 10000;
    [HideInInspector]
    public string randomID;
    [HideInInspector]
    public long timeOnStartup;

    private void Start()
    {
        playerCamera = GetComponentInChildren<Camera>();
        playerCamera.tag = "MainCamera";
        unitController = GetComponent<UnitController>();
        playerColor = GameManager.instance.playerColors[playerNumber];
        SetMainCamera();
    }

    [ClientCallback]
    public void SetMainCamera()
    {
        playerCamera.tag = "MainCamera";
    }



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
                Debug.Log("Mouse Not Hitting Something");
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            GameObject clone = (GameObject)Instantiate(unitController.meleePrefab, mousePosition, Quaternion.identity);
            clone.GetComponent<Unit>().playerOwnership = 1;
            clone.tag = GameManager.instance.playerTags[1];
            clone.GetComponent<UnitColor>().playerNumber = 1;
            NetworkServer.Spawn(clone);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            GameObject clone = (GameObject)Instantiate(unitController.meleePrefab, mousePosition, Quaternion.identity);
            clone.GetComponent<Unit>().playerOwnership = 2;
            clone.tag = GameManager.instance.playerTags[2];
            clone.GetComponent<UnitColor>().playerNumber = 2;
            NetworkServer.Spawn(clone);
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            GameObject clone = (GameObject)Instantiate(unitController.rangedPrefab, mousePosition, Quaternion.identity);
            clone.GetComponent<Unit>().playerOwnership = 1;
            clone.tag = GameManager.instance.playerTags[1];
            clone.GetComponent<UnitColor>().playerNumber = 1;
            NetworkServer.Spawn(clone);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            GameObject clone = (GameObject)Instantiate(unitController.rangedPrefab, mousePosition, Quaternion.identity);
            clone.GetComponent<Unit>().playerOwnership = 2;
            clone.tag = GameManager.instance.playerTags[2];
            clone.GetComponent<UnitColor>().playerNumber = 2;
            NetworkServer.Spawn(clone);
        }
    }
}
