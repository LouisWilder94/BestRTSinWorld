using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingType
{
    Barracks
}

public enum UnitType
{
    Melee
}

public enum MonsterType
{
    Monster
}

public enum UpgradeType
{
    Powerup
}

public class Factory : MonoBehaviour {
    //singleton stuff
    private static Factory _instance;
    public static Factory instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<Factory>();
            return _instance;
        }
    }
    //singleton stuff

    public Material wireframeMaterial;
    public GameObject barracksPrefab;

    GameObject buildingPreview;

    delegate void AttachBuildingToCursor(GameObject buildingPreview, BuildingType type);
    AttachBuildingToCursor attachBuildToCursor;

    private void Start()
    {
        BuildingPlace buildingPlaceScript = FindObjectOfType<BuildingPlace>();
        if (buildingPlaceScript != null)
            attachBuildToCursor += buildingPlaceScript.AttachPreviewToCursor;
    }

    public void PreviewBuilding(BuildingType type)
    {
        GameObject previewPrefab;

        switch (type)
        {
            case BuildingType.Barracks:
                previewPrefab = barracksPrefab;
                break;
            default:
                previewPrefab = null;
                break;
        }

        buildingPreview = Instantiate(previewPrefab);
        buildingPreview.GetComponent<Renderer>().material = wireframeMaterial;
        buildingPreview.GetComponent<Collider>().enabled = false;
        buildingPreview.GetComponent<Building>().enabled = false;

        if (attachBuildToCursor != null)
            attachBuildToCursor(buildingPreview, type);
    }

    public void CreateBuilding(Vector3 position, BuildingType type)
    {
        GameObject buildingPrefab;

        switch (type)
        {
            case BuildingType.Barracks:
                buildingPrefab = barracksPrefab;
                break;
            default:
                buildingPrefab = null;
                break;
        }

        GameObject building = Instantiate(buildingPrefab, position, Quaternion.identity);
    }

    public void CreateUnit(string unitName)
    {
        
    }

    public GameObject CreateMonster(string monsterName)
    {
        return null;
    }
}
