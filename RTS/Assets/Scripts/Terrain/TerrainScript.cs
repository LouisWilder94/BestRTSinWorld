using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class TerrainScript : NetworkBehaviour, IPointerClickHandler {

    private PointerEventData eventDataTemp;

    [ServerCallback]
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            eventDataTemp = eventData;
            Debug.Log("Right-Click");
            SelectionScript.instance.MoveOut(eventData.pointerPressRaycast.worldPosition);
            //RpcMoveout();
        }
    }

    [ClientRpc]
    public void RpcMoveout()
    {
        RpcMoveout();
    }

    public void _moveOut()
    {
        SelectionScript.instance.MoveOut(eventDataTemp.pointerPressRaycast.worldPosition);
    }
}
