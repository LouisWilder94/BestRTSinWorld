using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicUI : MonoBehaviour {
    // This script is for changing the context buttons that pop up based on what you've clicked on. 
    // Eventually it will also handle the selection window, which will show the stats and health of the units you have selected.
    // Currently it only interacts with the SelectionScript through a delegate.

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

    public GameObject buttonPrefab;
    public GameObject topPanel;
    public GameObject bottomPanel;

    List<Button> buttons = new List<Button>();

    // Reference to the BuildingPlace script.
//    delegate void StartBuildingPreview()

    // UpdateUI is only called through the SelectionScript when a unit is selected or deselected.
    // Currently this system only works with one unit at a time. My assumption is that we'll change 
    // change the selectionScript to sort the things that you've selected, and only let it select
    // one type of object, like units / buildings, etc.
    public void UpdateUI(List<SelectableUnitComponent> selectedObjects)
    {
        Debug.Log("UpdateUI was called.");

        if (selectedObjects.Count > 0)
        {
            Debug.Log("There were units selected.");
            if (selectedObjects[0].GetComponent<Unit>() != null)
            {
                if (selectedObjects[0].GetComponent<Builder>() != null)
                {
                    ButtonCache barracksButton = Instantiate(buttonPrefab, topPanel.transform).GetComponent<ButtonCache>();
                    barracksButton.textComponent.text = "Barracks";
                    barracksButton.buttonComponent.onClick.AddListener(TestMethod);
                }
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

    void TestMethod()
    {
        Debug.Log("Button method called.");
    }
}
