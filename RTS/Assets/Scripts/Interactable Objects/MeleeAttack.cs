using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MeleeAttack : NetworkBehaviour {

    [SerializeField]
    float attackRange = 3f;
    [SerializeField]
    float attackSpeed = 1f;
    [SerializeField]
    int damage = 15;

    GameObject currentTarget;
    UnitHealth targetHealth;

	void Start ()
    {
        StartCoroutine(AttackTimer());
	}
	
	void Update ()
    {
        if (currentTarget == null || Vector3.Distance(gameObject.transform.position, currentTarget.transform.position) > attackRange)
        {
            currentTarget = FindClosestEnemy();
        }


        if (currentTarget != null)
        {
            LookAtTarget();
        }
    }

    GameObject FindClosestEnemy()
    {
        GameObject[] gos;
        if(gameObject.tag == "Player1")
            gos = GameObject.FindGameObjectsWithTag("Player2");
        else
            gos = GameObject.FindGameObjectsWithTag("Player1");

        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }

    bool CheckRange()
    {
        if(currentTarget != null)
        {
            float distanceToEnemy;
            distanceToEnemy = Vector3.Distance(gameObject.transform.position, currentTarget.transform.position);

            return distanceToEnemy <= attackRange;
        }
        return false;
    }

    void LookAtTarget()
    {
        if (Vector3.Distance(gameObject.transform.position, currentTarget.transform.position) < attackRange * 3)
        {
            Vector3 targetPoint;
            Quaternion targetRotation;

            targetPoint = new Vector3(currentTarget.transform.position.x, transform.position.y, currentTarget.transform.position.z) - transform.position;
            targetRotation = Quaternion.LookRotation(-targetPoint, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2.0f);
        }
    }

    void AttackTarget()
    {

        targetHealth = currentTarget.GetComponent<UnitHealth>();

        //gameObject.transform.LookAt(currentTarget.transform);

        targetHealth.TakeDamage(damage);
    }

    IEnumerator AttackTimer()
    {
        while (true)
        {
            if (CheckRange())
            {
                AttackTarget();
                Debug.Log("Attacked" + currentTarget);
            }
            yield return new WaitForSeconds(attackSpeed);
        }
        yield return null;
    }
}
