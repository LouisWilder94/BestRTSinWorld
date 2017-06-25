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
    ////singleton stuff
    //private static Factory _instance;
    //public static Factory instance
    //{
    //    get
    //    {
    //        if (_instance == null)
    //            _instance = FindObjectOfType<Factory>();
    //        return _instance;
    //    }
    //}
    ////singleton stuff

    public Material wireframeMaterial;
    public GameObject barracksPrefab;

    private GameObject BuildingPrefabBank(BuildingType type)
    {
        GameObject buildingPrefab;

        switch (type)
        {
            case BuildingType.Barracks:
                buildingPrefab = barracksPrefab;
                break;
            default:
                Debug.LogWarning("Factory was not passed a valid building type.");
                buildingPrefab = null;
                break;
        }

        return buildingPrefab;
    }

    public GameObject CreatePreviewBuilding(BuildingType type)
    {
        GameObject previewPrefab = BuildingPrefabBank(type);

        GameObject buildingPreview = Instantiate(previewPrefab);
        buildingPreview.GetComponent<Renderer>().material = wireframeMaterial;
        //buildingPreview.GetComponent<Collider>().isTrigger = true;
        //buildingPreview.GetComponent<Collider>().enabled = false;
        //buildingPreview.GetComponent<Building>().enabled = false;

        return buildingPreview;
    }

    public void CreateBuilding(Vector3 position, BuildingType type)
    {
        GameObject buildingPrefab = BuildingPrefabBank(type);

        GameObject building = Instantiate(buildingPrefab, position, Quaternion.identity);
    }

    public GameObject meleePrefab;

    public void CreateUnit(Vector3 position, UnitType type)
    {
        GameObject unitPrefab;

        switch (type)
        {
            case UnitType.Melee:
                unitPrefab = meleePrefab;
                break;
            default:
                Debug.LogWarning("Factory was not passed a valid unit type.");
                unitPrefab = null;
                break;
        }

        GameObject unit = Instantiate(unitPrefab, position, Quaternion.identity);
    }

    public GameObject CreateMonster(MonsterType type)
    {
        return null;
    }
}
