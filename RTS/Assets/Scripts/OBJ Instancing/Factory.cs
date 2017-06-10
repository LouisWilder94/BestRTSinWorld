using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public GameObject CreateBuilding(string buildingName)
    {
        switch (buildingName)
        {
            case "Barracks":
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
