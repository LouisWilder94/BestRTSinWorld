using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacing : MonoBehaviour
{
    static GameObject selectedBuilding;
    static GameObject previewBuilding;

    public GameObject prefab;

    public void AttachBuildingToCursor(GameObject building)
    {
        selectedBuilding = building;
    }

    Vector3 mouseSpot;

    private void Awake()
    {
        previewBuilding = Instantiate(prefab);
        previewBuilding.SetActive(false);
        previewBuilding.GetComponent<Collider>().enabled = false;
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            previewBuilding.SetActive(true);
            previewBuilding.transform.position = hit.point;
            if (Input.GetMouseButtonDown(0))
            {
                if (selectedBuilding != null)
                    Instantiate(selectedBuilding, hit.point, Quaternion.identity);
            }
        }
        else
            previewBuilding.SetActive(false);
    }
}
