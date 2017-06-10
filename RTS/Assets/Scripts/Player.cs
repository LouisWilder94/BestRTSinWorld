using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {
    public Camera playerCamera;
    public int Money;

    [HideInInspector]
    public int playerNumber;

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



    private void Awake()
    {
        GameManager.instance.AddPlayerToList(this);
    }
}
