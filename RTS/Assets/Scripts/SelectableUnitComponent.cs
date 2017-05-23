using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;


public class SelectableUnitComponent : MonoBehaviour
{
    public GameObject selectionCircle;
    public float moveSpeed = 1f;


    public void moveTo(Vector3 targetPosition)
    {
        StartCoroutine(Move(targetPosition));
    }
    public IEnumerator Move(Vector3 targetPosition)
    {
        float startTime = Time.time;
        float amount;

        while ((amount = (Time.time - startTime)) < 1)
        {
            transform.LookAt(new Vector3(targetPosition.x,transform.position.y,targetPosition.z));
            transform.Translate(transform.forward * moveSpeed * Time.deltaTime);

            yield return null;
        }

        
    }
}