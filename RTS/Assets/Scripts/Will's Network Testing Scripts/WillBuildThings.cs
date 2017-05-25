using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WillBuildThings : NetworkBehaviour
{
    public Material wireframeMaterial;

    GameObject selectedBuilding;
    Material startingMaterial;

    public GameObject willsTestGO;

    //method runs when UI button is clicked
    public void AttachBuildingToCursor(GameObject building)
    {
        Debug.Log("Button press.");

        selectedBuilding = Instantiate(building);
        startingMaterial = selectedBuilding.GetComponent<Renderer>().material;
        selectedBuilding.GetComponent<Renderer>().material = wireframeMaterial;
        selectedBuilding.GetComponent<Collider>().enabled = false;
    }

    Vector3 mouseSpot;

    void Update()
    {

        if (selectedBuilding != null)
        {
            //checks for the local player before spawning the building otherwise it returns
            //Will, this was breaking my testing, but probably because I didn't put the script on the player and put it on the UI instead
            //if (!isLocalPlayer)
            //    return;

            //variables for mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            //checks if mouse is over terrain
            if (Physics.Raycast(ray, out hit))
            {
                selectedBuilding.SetActive(true);

                //make preview follow the mouse and fake the grid-like placement
                selectedBuilding.transform.position = new Vector3((int)hit.point.x, (int)hit.point.y + selectedBuilding.GetComponent<Collider>().bounds.extents.y, (int)hit.point.z);

                //placing the building
                if (Input.GetMouseButtonDown(0))
                {
                    selectedBuilding.GetComponent<Renderer>().material = startingMaterial;

                    CmdSpawnBuilding(selectedBuilding, hit.point, Quaternion.identity);

                    selectedBuilding.SetActive(false);
                    selectedBuilding = null;
                }

                if (Physics.Raycast(ray, out hit))
                {
                    if (Input.GetKeyDown(KeyCode.Alpha6))
                    {
                        CmdSpawnBuilding(willsTestGO, hit.point, Quaternion.identity);
                    }
                }
            }
            else
                selectedBuilding.SetActive(false);
        }
    }


    //This sends a command to the server to create the building
    [Command]
    void CmdSpawnBuilding(GameObject building, Vector3 hitLocation, Quaternion rotation)
    {
        Debug.Log("test");
        Debug.Log("current building: " + building);

        GameObject instance = Instantiate(building, hitLocation, rotation) as GameObject; //I switched this back to not a prefab. The "prefab" basically exists on the UI button.

        NetworkServer.Spawn(instance);
    }
}