using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingType
{
    Barracks
}

public enum UnitType
{
    Solider
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

    public GameObject barracksPrefab;
    public GameObject CreateBuilding(BuildingType type)
    {
        switch (type)
        {
            case BuildingType.Barracks:
                return barracksPrefab;
            default:
                return null;
        }
    }

    public GameObject CreateUnit(string unitName)
    {
        return null;
    }

    public GameObject CreateMonster(string monsterName)
    {
        return null;
    }
}
