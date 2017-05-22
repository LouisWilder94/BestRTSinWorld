using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BuildingPlacing : NetworkBehaviour
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
        //checks for the local player before spawning the building otherwise it returns
        if (!isLocalPlayer)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            previewBuilding.SetActive(true);
            previewBuilding.transform.position = hit.point;

            

            if (Input.GetMouseButtonDown(0))
            {
                if (selectedBuilding != null)
                    CmdSpawnBuilding(selectedBuilding, hit.point, Quaternion.identity);
            }
        }
        else
            previewBuilding.SetActive(false);
    }


    //This sends a command to the server to create the building
    [Command]
    void CmdSpawnBuilding(GameObject building, Vector3 hitLocation, Quaternion rotation)
    {
        GameObject instance = Instantiate(prefab, hitLocation, rotation) as GameObject;           //It is currently instantiating prefab instead of selectedBuilding  <-------------

        NetworkServer.Spawn(instance);
    }
}
