using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;


public class SelectableUnitComponent : MonoBehaviour
{
    public GameObject selectionCircle;
    public float moveSpeed = 1f;

    //This only calls when the terrain is clicked on
    public void moveTo(Vector3 targetPosition)
    {
        StartCoroutine(Move(targetPosition));
    }

    public IEnumerator Move(Vector3 targetPosition)
    {
        yield return null;

        //targetPosition.y = transform.position.y;

        //while (transform.position != targetPosition)
        //{
        //    transform.LookAt(targetPosition);
        //    transform.Translate(transform.forward * moveSpeed * Time.deltaTime);

        //    yield return null;
        //}     
    }
}