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
    delegate void FactoryCreateBuilding(Vector3 position, BuildingType type);
    FactoryCreateBuilding onCallCreateBuilding;
    delegate GameObject FactoryCreatePreview(BuildingType type);
    FactoryCreatePreview onCallCreatePreview;

    private void Start()
    {
        Factory factoryScript = FindObjectOfType<Factory>();
        if (factoryScript != null)
            onCallCreatePreview += factoryScript.CreatePreviewBuilding;
            onCallCreateBuilding += factoryScript.CreateBuilding;          
    }

    //method runs when UI button is clicked
    public void AttachPreviewToCursor(BuildingType type)
    {      
        selectedBuilding = onCallCreatePreview(type);
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
            if (Physics.Raycast(ray, out hit) && !UIClickBlocker.isHovered)
            {
                selectedBuilding.SetActive(true);

                if (hit.transform.gameObject.layer == 8)
                {
                    //make preview follow the mouse and fake the grid-like placement
                    selectedBuilding.transform.position = new Vector3((int)hit.point.x, ((int)hit.point.y + selectedBuilding.GetComponent<MeshRenderer>().bounds.extents.y), (int)hit.point.z);
                }

                //placing the building
                if (Input.GetMouseButtonDown(0))
                {
                    if (onCallCreateBuilding != null)
                        onCallCreateBuilding(selectedBuilding.transform.position, type);

                    Destroy(selectedBuilding);
                }
            }
            else
                selectedBuilding.SetActive(false);

            yield return null;
        }
    }
}
