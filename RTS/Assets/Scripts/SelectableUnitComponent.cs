using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

public class SelectableUnitComponent : MonoBehaviour
{
    public GameObject selectionCircle;
    public float moveSpeed = 1f;

    public Vector3 target;
    NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void moveTo(Vector3 targetPosition)
    {
        agent.SetDestination(targetPosition);
    }
}