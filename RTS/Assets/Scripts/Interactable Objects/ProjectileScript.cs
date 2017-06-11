using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ProjectileScript : NetworkBehaviour {

    Vector3 startPos;
    Vector3 targetPos;

    public GameObject targetObject;

    float timer;

	void Start () {
        startPos = gameObject.transform.position;
        //targetObject = FindClosestEnemy();
	}

    void Update()
    {
        if(targetObject == null)
        {
            NetworkServer.Destroy(gameObject);
        }

        timer += Time.deltaTime;

        targetPos = targetObject.transform.position;
        transform.position = Vector3.Lerp(startPos, targetPos, timer * 2f);

        if(timer >= 1f)
        {
            NetworkServer.Destroy(gameObject);
        }
    }

    //GameObject FindClosestEnemy()
    //{
    //    GameObject[] gos;
    //    if (gameObject.tag == "Player1Projectile")
    //        gos = GameObject.FindGameObjectsWithTag("Player2");
    //    else
    //        gos = GameObject.FindGameObjectsWithTag("Player1");

    //    GameObject closest = null;
    //    float distance = Mathf.Infinity;
    //    Vector3 position = transform.position;
    //    foreach (GameObject go in gos)
    //    {
    //        Vector3 diff = go.transform.position - position;
    //        float curDistance = diff.sqrMagnitude;
    //        if (curDistance < distance)
    //        {
    //            closest = go;
    //            distance = curDistance;
    //        }
    //    }
    //    return closest;
    //}
}
