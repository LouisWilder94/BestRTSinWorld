using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductionBuilding : Building {
    public GameObject spawnPoint;

	void Start () {
        spawnPoint = new GameObject();
        spawnPoint.transform.parent = gameObject.transform;
        spawnPoint.name = "Spawn Point";
        spawnPoint.transform.position = new Vector3(gameObject.GetComponent<Collider>().bounds.min.x + -0.5f, gameObject.transform.position.y - gameObject.GetComponent<Collider>().bounds.extents.y, gameObject.GetComponent<Collider>().bounds.min.z + -0.5f);
    }
}
