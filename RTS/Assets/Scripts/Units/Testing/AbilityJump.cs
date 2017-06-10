using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AbilityJump : MonoBehaviour
{
    private Rigidbody thisRigidBody;
    private NavMeshAgent thisAgent;

    [Header("Attributes")]
    public float jumpPower = 800f;
    public float speed = 20f;
    public float jumpCompleteRate = 1f;

    [Header("Art")]
    public GameObject impactEffect;
    public GameObject highlightEffect;
    public float impactEffectTime = 1f;

    private bool canJump = true;

    public void Start()
    {
        thisRigidBody = GetComponent<Rigidbody>();
        thisAgent = GetComponent<NavMeshAgent>();
        canJump = true;
    }

    public void Jump(Vector3 position)
    {

        StartCoroutine(movementHandling(position));

    }

    public IEnumerator movementHandling(Vector3 position)
    {
        float time = 0f;
        thisAgent.enabled = false;
        thisRigidBody.drag = 0f;
        thisRigidBody.AddRelativeForce(Vector3.up * jumpPower * 1000f);
        thisRigidBody.useGravity = true;

        GameObject fx1 = (GameObject)Instantiate(highlightEffect, position, Quaternion.identity);

        while (true)
        {

            if ((transform.position - position).magnitude <= 10f)
            {
                thisAgent.enabled = true;
                canJump = true;
                thisRigidBody.useGravity = false;
                thisAgent.SetDestination(transform.position);
                //thisRigidBody.drag = 5f;
                GameObject fx2 = (GameObject)Instantiate(impactEffect, (transform.position + Vector3.up * -2f), Quaternion.identity);
                Destroy(fx1);
                Destroy(fx2, impactEffectTime);
                yield return new WaitForSeconds(impactEffectTime + 0.1f);
                yield break;
            }





            time += Time.fixedDeltaTime;

            float velRate = 1/ (time / jumpCompleteRate);

            //thisRigidBody.AddRelativeForce(Vector3.up * velRate);
            thisRigidBody.AddForce((position - transform.position).normalized * speed);



            canJump = false;



            Debug.Log(velRate);
            yield return new WaitForFixedUpdate();
        }
    }
}
