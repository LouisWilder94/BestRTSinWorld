using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIClickBlocker : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static bool isHovered;
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Started hovering");
        isHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Stopped hovering");
        isHovered = false;
    }
}
