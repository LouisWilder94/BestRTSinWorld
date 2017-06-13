using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BuildingPlace : MonoBehaviour
{
    GameObject selectedBuilding;
    Material startingMaterial;
    BuildingType type;

    // Reference to the Factory script.
    delegate void FactoryHandler(Vector3 position, BuildingType type);
    FactoryHandler callCreateBuilding;

    private void Start()
    {
        Factory factoryScript = FindObjectOfType<Factory>();
        if (factoryScript != null)
            callCreateBuilding += factoryScript.CreateBuilding;
    }

    //method runs when UI button is clicked
    public void AttachPreviewToCursor(GameObject building, BuildingType type)
    {
        this.type = type;
        selectedBuilding = building;
        StartCoroutine(BuildingPreview());
    }

    Vector3 mouseSpot;

    IEnumerator BuildingPreview()
    {
        while (selectedBuilding != null)
        {
            //variables for mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            //checks if mouse is over terrain
            if (Physics.Raycast(ray, out hit))
            {
                selectedBuilding.SetActive(true);

                //make preview follow the mouse and fake the grid-like placement
                selectedBuilding.transform.position = new Vector3((int)hit.point.x, ((int)hit.point.y + selectedBuilding.GetComponent<MeshRenderer>().bounds.extents.y), (int)hit.point.z);

                //placing the building
                if (Input.GetMouseButtonDown(0))
                {
                    if (callCreateBuilding != null)
                        callCreateBuilding(selectedBuilding.transform.position, type);

                    Destroy(selectedBuilding);
                }
            }
            else
                selectedBuilding.SetActive(false);

            yield return null;
        }
    }
}
