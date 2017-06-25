using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Reflection;

public class SelectionScript : MonoBehaviour {
    //singleton stuff
    static SelectionScript _instance;
    public static SelectionScript instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<SelectionScript>();
            return _instance;
        }
    }
    //singleton stuff

    public GameObject selectionCirclePrefab;
    public List<SelectableUnitComponent> selectedObjects;

    //for selecting units
    bool isSelecting = false;
    Vector3 mousePosition1;

    //Movement 
    delegate void MovementDelegate(Vector3 position);
    MovementDelegate movementDelegate;

    //Updating UI
    delegate void DynamicUIUpdate(List<SelectableUnitComponent> selectedObjects);
    DynamicUIUpdate onCallUpdateUI;

    private void Start()
    {
        //this is for registering the dynamic UI script with this script
        DynamicUI dynamicUIscript = FindObjectOfType<DynamicUI>();
        if (dynamicUIscript != null)
            onCallUpdateUI += dynamicUIscript.UpdateUI;
    }

    public StandaloneInputModule module;

    void Update()
    {
        if (!UIClickBlocker.isHovered)
        {
            // If we press the left mouse button, save mouse location and begin selection
            if (Input.GetMouseButtonDown(0))
            {
                isSelecting = true;
                mousePosition1 = Input.mousePosition;
                movementDelegate = null;
                foreach (var selectableObject in FindObjectsOfType<SelectableUnitComponent>())
                {
                    if (selectableObject.selectionCircle != null)
                    {
                        Destroy(selectableObject.selectionCircle.gameObject);
                        selectableObject.selectionCircle = null;
                    }
                }
            }
            // If we let go of the left mouse button, end selection
            if (Input.GetMouseButtonUp(0))
            {
                selectedObjects = new List<SelectableUnitComponent>();
                foreach (var objects in FindObjectsOfType<SelectableUnitComponent>())
                {
                    if (IsWithinSelectionBounds(objects.gameObject))
                    {
                        selectedObjects.Add(objects);
                        movementDelegate += objects.moveTo;
                    }
                }

                foreach (var objects in selectedObjects)
                {
                    Debug.Log(objects.gameObject.name);
                }

                //this calls the Update UI method in the dynamicUI
                if (onCallUpdateUI != null)
                    onCallUpdateUI(selectedObjects);

                isSelecting = false;
            }
        }

        if (isSelecting)
        {
            foreach( var selectableObject in FindObjectsOfType<SelectableUnitComponent>())
            {
                if (IsWithinSelectionBounds(selectableObject.gameObject))
                {
                    if (selectableObject.selectionCircle == null)
                    {
                        selectableObject.selectionCircle = Instantiate(selectionCirclePrefab);
                        selectableObject.selectionCircle.transform.SetParent(selectableObject.transform, false);
                        selectableObject.selectionCircle.transform.eulerAngles = new Vector3(90, 0, 0);
                    }
                }
                else
                {
                    if(selectableObject.selectionCircle != null)
                    {
                        Destroy(selectableObject.selectionCircle.gameObject);
                        selectableObject.selectionCircle = null;
                    }
                }
            }
        }
    }

    void OnGUI()
    {
        if (isSelecting)
        {
            // Create a rect from both mouse positions
            Rect rect = Utils.GetScreenRect(mousePosition1, Input.mousePosition);
            Utils.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
            Utils.DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
        }
    }
    public bool IsWithinSelectionBounds(GameObject gameObject)
    {
        if (!isSelecting)
        {
            return false;
        }
        Camera camera = Camera.main;
        Bounds viewportBounds = Utils.GetViewportBounds(camera, mousePosition1, Input.mousePosition);

        return viewportBounds.Contains(camera.WorldToViewportPoint(gameObject.transform.position));
    }
    public void MoveOut(Vector3 targetPosition)
    {
        Debug.Log("MoveOut");
        movementDelegate(targetPosition);       
    }
}
