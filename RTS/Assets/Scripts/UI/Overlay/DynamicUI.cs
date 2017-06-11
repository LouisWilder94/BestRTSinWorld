using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicUI : MonoBehaviour {
    ////singleton stuff
    //private static DynamicUI _instance;
    //public static DynamicUI instance
    //{
    //    get
    //    {
    //        if (_instance == null)
    //            _instance = FindObjectOfType<DynamicUI>();
    //        return _instance;
    //    }
    //}
    ////singleton stuff

    List<Button> buttons = new List<Button>();

    //is called when a new unit is clicked on, or when all units are deselected
    public void UpdateUI(List<SelectableUnitComponent> selectedObjects)
    {
        Debug.Log("UpdateUI was called.");

        if (selectedObjects.Count > 0)
        {
            Debug.Log("There were units selected.");
            if (selectedObjects[0].GetComponent<Unit>() != null)
            {
                
            }

            if (selectedObjects[0].GetComponent<Building>() != null)
            {

            }

            if (selectedObjects[0].GetComponent<Monster>() != null)
            {

            }

            if (selectedObjects[0].GetComponent<Upgrade>() != null)
            {

            }
        }
        else
        {
            Debug.Log("No units were selected.");
            foreach (Button button in buttons)
            {
                button.gameObject.SetActive(false);
            }
        }
    }
}
