using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.AI;

public class Unit : NetworkBehaviour
{
    [Header("Attributes")]
    public float basicAttackDamage = 10f;
    public float basicAttackCD = 1f;

    public float heavyAttackDamage = 20f;
        public float heavyAttackCd = 2f;

    [Header("Startup")]
    public NavMeshAgent navAgent;
    public UnitAnimator unitAnimator;
    public UnitHealth healthScript;

    [Header("Behaviors")]
    public bool dodge = false;
    public float dodgePower;

    public bool Blocks = false;

    public float navmeshDestinationReachDist = 2f;

    public int playerOwnership;


    public float maxSpeed
    {
        get
        {
            return navAgent.speed;
        }
        set
        {
            navAgent.speed = value;
        }
    }

    public float angularSpeed
    {
        get
        {
            return navAgent.angularSpeed;
        }
        set
        {
            navAgent.angularSpeed = value;
        }
    }

    public float currentSpeed
    {
        get
        {
            return navAgent.velocity.sqrMagnitude;
        }
        set
        {
            currentSpeed = value;
            navAgent.velocity = navAgent.velocity.normalized * value;
        }
    } 


    public bool moving
    {
        get
        {
            return moving;
        }
        set
        {
            if (value = moving)
                return;

            moving = value;
        }
    }

    public bool incombat;

    void SetIncombat(bool _bool)
    {
        incombat = _bool;
        unitAnimator.SetBool("InCombat", _bool);
    }

    public bool blocking;


    public void ToggleBlock()
    {
        blocking = !blocking;
        unitAnimator.SetBool("Block", blocking);
    }

    private void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        unitAnimator = gameObject.GetComponent<UnitAnimator>();
    }

    public void Move(Vector3 position)
    {
        navAgent.SetDestination(position);
    }

    public void CheckDist()
    {
        if ((transform.position - navAgent.destination).magnitude <= navmeshDestinationReachDist)
            navAgent.destination = transform.position;
    }

    public void Attack(UnitHealth target)
    {
        target.TakeDamage(basicAttackDamage);
        unitAnimator.randomAttack1();
    }

    public void HeavyAttack(UnitHealth target)
    {
        target.TakeDamage(heavyAttackDamage);
        unitAnimator.randomAttack2();
    }


    public void Dodge(Vector3 objToAvoid, float power)
    {
        Vector3 currentPos = transform.position;
        Vector3 angle = (currentPos - objToAvoid).normalized;
        Vector3 dodgePos = angle * power;
        //navAgent.SetDestination(dodgePos);
        navAgent.velocity = dodgePos;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Dodge(FunctionsHelper.GetCursorPosition(playerOwnership), dodgePower);

            Debug.Log(FunctionsHelper.GetCursorPosition(playerOwnership));
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            HeavyAttack(SelectionScript.instance.selectedObjects[Random.Range(0, SelectionScript.instance.selectedObjects.Count)].gameObject.GetComponent<UnitHealth>());
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ToggleBlock();
        }
        if(Input.GetKeyDown(KeyCode.T))
        {
            Attack(SelectionScript.instance.selectedObjects[Random.Range(0, SelectionScript.instance.selectedObjects.Count)].gameObject.GetComponent<UnitHealth>());
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            SetIncombat(!incombat);
        }
    }
}
