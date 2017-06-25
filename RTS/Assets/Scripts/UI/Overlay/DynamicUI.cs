using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicUI : MonoBehaviour {
    // This script is for changing the context buttons that pop up based on what you've clicked on. 
    // Eventually it will also handle the selection window, which will show the stats and health of the units you have selected.
    // Interacts with (through delegates): BuildingPlace, Factory

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

    // References to the Factory script.
    delegate void BuildingPreview(BuildingType type);
    BuildingPreview onCallPreviewBuilding;
    delegate void FactoryUnit(Vector3 position, UnitType type);
    FactoryUnit onCallCreateUnit;

    private void Start()
    {
        BuildingPlace buildingPlaceScript = FindObjectOfType<BuildingPlace>();
        if (buildingPlaceScript != null)
            onCallPreviewBuilding += buildingPlaceScript.AttachPreviewToCursor;

        Factory factoryScript = FindObjectOfType<Factory>();
        if (factoryScript != null)
            onCallCreateUnit += factoryScript.CreateUnit;
    }

    // UpdateUI is only called through the SelectionScript when a unit is selected or deselected.
    // Currently this system only works with one unit at a time. My assumption is that we'll change 
    // change the selectionScript to sort the things that you've selected, and only let it select
    // one type of object, like units / buildings, etc.
    public void UpdateUI(List<SelectableUnitComponent> selectedObjects)
    {
        Debug.Log(selectedObjects.Count);

        if (selectedObjects.Count > 0)
        {
            if (selectedObjects[0].GetComponent<Unit>() != null)
            {
                // if unit is builder
                if (selectedObjects[0].GetComponent<Builder>() != null)
                {
                    ButtonCache barracksButton = Instantiate(buttonPrefab, topPanel.transform).GetComponent<ButtonCache>();
                    barracksButton.textComponent.text = "Barracks";

                    if (onCallPreviewBuilding != null)
                        barracksButton.buttonComponent.onClick.AddListener(delegate { onCallPreviewBuilding(BuildingType.Barracks); } );

                    buttons.Add(barracksButton.buttonComponent);
                }
            }

            if (selectedObjects[0].GetComponent<Building>() != null)
            {
                // if building is barracks
                if (selectedObjects[0].GetComponent<Barracks>() != null)
                {
                    ButtonCache meleeButton = Instantiate(buttonPrefab, topPanel.transform).GetComponent<ButtonCache>();
                    meleeButton.textComponent.text = "Melee";

                    if (onCallCreateUnit != null)
                        meleeButton.buttonComponent.onClick.AddListener(delegate { onCallCreateUnit(selectedObjects[0].GetComponent<ProductionBuilding>().spawnPoint.transform.position, UnitType.Melee); });

                    buttons.Add(meleeButton.buttonComponent);
                }
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
            foreach (Button button in buttons)
            {
                Destroy(button.gameObject);
            }

            buttons.Clear();
        }
    }
}
