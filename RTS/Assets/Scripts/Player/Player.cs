using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {
    public Camera playerCamera;
    public int Minerals;
   //[HideInInspector]
    public int playerNumber = 10000;

    public float randomID;

    

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
        GameManager.instance.RpcAddPlayerToList(this);
        randomID = Random.Range(0, 100f);
    }
}
