using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicUI : MonoBehaviour {
    //singleton stuff
    private static DynamicUI _instance;
    public static DynamicUI instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<DynamicUI>();
            return _instance;
        }
    }
    //singleton stuff

    List<Button> buttons = new List<Button>();

    //creates a list of the dynamic buttons and sets them all to interactable 
    private void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            buttons.Add(transform.GetChild(i).GetComponent<Button>());
            buttons[i].interactable = false;
        }
    }

    //is called when a new unit is clicked on, or when all units are deselected
    public void UpdateUI()
    {
        Debug.Log("UpdateUI was called.");

        if (SelectionScript.instance.selectedObjects.Count > 0)
        {
            Debug.Log("There were units selected.");
            if (SelectionScript.instance.selectedObjects[0].GetComponent<Unit>() != null)
            {
                Debug.Log("Selected unit was a unit.");
                buttons[0].GetComponentInChildren<Text>().text = "Move";
                buttons[0].interactable = true;
            }

            if (SelectionScript.instance.selectedObjects[0] is Building)
            {

            }

            if (SelectionScript.instance.selectedObjects[0] is Monster)
            {

            }

            if (SelectionScript.instance.selectedObjects[0] is Upgrade)
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
