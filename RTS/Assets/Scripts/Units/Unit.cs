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

    public float meleeRange = 6f;

    [HideInInspector]
    public float startSpeed;

    [HideInInspector]
    public float startAcceleration;

    [HideInInspector]
    public float StartAngularSpeed;

    [Header("Startup")]
    public NavMeshAgent navAgent;
    public UnitAnimator unitAnimator;
    public UnitHealth healthScript;

    [Header("Behaviors")]
    public bool dodge = false;
    public float dodgePower;

    public float strafeDist = 4f;
    public float StrafePower = 20f;

    public bool Blocks = false;

    public float navmeshDestinationReachDist = 2f;

    public int playerOwnership;

    [Header("Detection")]
    [HideInInspector]
    public BoxCollider detectCollider;
    public float detectDistance;

    public const float checkTime = 0.2f;



    //Setting navmesh calues
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
            navAgent.velocity = navAgent.velocity.normalized * value;
        }
    } 

    public float Acceleration
    {
        get
        {
            return navAgent.acceleration;
        }
        set
        {
            navAgent.acceleration = value;
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
            if (value == moving)
                return;

            moving = value;
        }
    }

    [HideInInspector]
    public bool incombat;


    //Setting States
    void SetIncombat(bool _bool)
    {
        incombat = _bool;
        unitAnimator.SetBool("InCombat", _bool);
    }

    [HideInInspector]
    public bool blocking;

    public void ToggleBlock()
    {
        blocking = !blocking;
        unitAnimator.SetBool("Block", blocking);
    }

    public void SetBlocking(bool value)
    {
        blocking = value;
        unitAnimator.SetBool("Block", blocking);
        healthScript.blocking = value;
    }

    public IEnumerator BlockForSeconds(float seconds)
    {
        SetBlocking(true);
        yield return new WaitForSeconds(seconds);
        SetBlocking(false);
    }

    private void Start()
    {
        StartAngularSpeed = navAgent.angularSpeed;
        startAcceleration = navAgent.acceleration;
        startSpeed = maxSpeed;

        navAgent = GetComponent<NavMeshAgent>();
        unitAnimator = gameObject.GetComponent<UnitAnimator>();
        CreateDetectionCollider();

      //  UnitHealth target = FunctionsHelper.GetNearestUnit(transform.position, healthScript);
     //   ChaseTarget(target, target.transform);
    }


    //Movement and navigation
    public void Move(Vector3 position)
    {
        navAgent.SetDestination(position);
    }

    public float CheckDist()
    {
        return (transform.position - navAgent.destination).magnitude;
    }

    public void Dodge(Vector3 objToAvoid, float power)
    {
        Vector3 currentPos = transform.position;
        Vector3 angle = (currentPos - objToAvoid).normalized;
        Vector3 dodgePos = angle * power;
        //navAgent.SetDestination(dodgePos);
        navAgent.velocity = dodgePos;
    }
    
    public void Knockback(Vector3 source, float power)
    {
        Vector3 dodgeAngle = (transform.position - source).normalized * power;
        navAgent.velocity = dodgeAngle;
    }

    public void Rush(Vector3 objToRush, float power)
    {
        Vector3 currentPos = transform.position;
        Vector3 angle = (objToRush - currentPos).normalized;
        Vector3 dodgePos = angle * power;
        //navAgent.SetDestination(dodgePos);
        navAgent.velocity = dodgePos;
    }

    public IEnumerator Strafe()
    {
        CompleteDest = true;
        Vector3 pos = GetStrafePos(strafeDist);
        navAgent.SetDestination(pos);
        while (true)
        {
            angularSpeed = 0f;
            //currentSpeed = 60f;
            maxSpeed = StrafePower;
            Acceleration = 100f;
            Debug.Log(angularSpeed);

            yield return new WaitForSeconds(checkTime);

            if (CheckDist() <= navmeshDestinationReachDist | navAgent.destination != pos)
            {
                angularSpeed = StartAngularSpeed;
                maxSpeed = startSpeed;
                Acceleration = startAcceleration;
                yield break;
            }

        }

    }


    bool CompleteDest = false;
    public IEnumerator SetDestination(Vector3 target)
    {
        while (true)
        {

            CompleteDest = true;
            navAgent.SetDestination(target);
            yield return new WaitForSeconds(checkTime);

            if (CheckDist() <= navmeshDestinationReachDist | navAgent.destination != target)
            {
                yield break;
            }
            
        }

    }

    public Vector3 GetStrafePos(float distance)
    {
        Vector3 dist;
        float random = Random.Range(0f, 2f);
        if (random <= 1f)
        {
            dist = Vector3.left * distance;
        }
        else
        {
            dist = Vector3.right * distance;
        }

        return ((Quaternion.AngleAxis(transform.rotation.y, Vector3.up) * dist) + transform.position);
    } 

    //Attacking
    [HideInInspector]
    public bool canAttack = true;

    public IEnumerator RechargeAttackTimer(float time)
    {
        canAttack = false;
        yield return new WaitForSeconds(time);
        canAttack = true;
    }

    public void Attack(UnitHealth target)
    {
        if (canAttack == false)
            return;

        unitAnimator.randomAttack1(target);
    }

    public void HeavyAttack(UnitHealth target)
    {
        if (canAttack == false)
            return;

        unitAnimator.randomAttack2(target);
    }



    //Target Handling
    public void CreateDetectionCollider()
    {
       Rigidbody dank = gameObject.AddComponent<Rigidbody>();
        dank.isKinematic = true;
        dank.useGravity = false;
        detectCollider = gameObject.AddComponent<BoxCollider>();
        detectCollider.center = new Vector3(0, 2, 0);
        detectCollider.size = new Vector3(detectDistance, detectDistance, detectDistance);
        detectCollider.isTrigger = true;
    }

    public void ChaseTarget(UnitHealth target, Transform targetPos)
    {
        if (hasTarget == true)
            return;

        StartCoroutine(Chase(target, targetPos));
        //FunctionsHelper.GetNearestTargetWithTag(GameManager.instance.playerTags[0], Vector3.up);
    }

    // public void OnTriggerStay(Collider other)
    // {
    //     if (hasTarget)
    //         return;
    //
    //     if (other.GetComponent<UnitHealth>() && other.tag != gameObject.tag) //&& other.tag != gameObject.tag)
    //     {
    //        Debug.Log(other);
    //       StartCoroutine(Chase(other.GetComponent<UnitHealth>(), other.transform));
    //    }
    // }

    public void OnTriggerEnter(Collider other)
    {
             if (hasTarget)
                 return;
        
             if (other.GetComponent<UnitHealth>() && other.tag != gameObject.tag) //&& other.tag != gameObject.tag)
             {
                Debug.Log(other);
               StartCoroutine(Chase(other.GetComponent<UnitHealth>(), other.transform));
            }
    }

    bool hasTarget = false;
    bool isInRange = false;
    //bool breakTarget = false;
    public UnitHealth target;
    public Transform targetPos;

    int checks;

    public IEnumerator Chase(UnitHealth _target, Transform _targetPos)
    {
        try
        {
            if (_target == null)
            {
                SetBlocking(false);
                SetIncombat(false);
                hasTarget = false;
                Debug.Log("Lost Target");
                yield break;
            }
        }
        catch
        {
            Debug.LogError("No target on starup");
        }


        target = _target;
        targetPos = _targetPos;
        checks = 0;

        while (true)
        {
            try
            {
                navAgent.destination = _targetPos.position;
            }
            catch
            {
                Debug.LogError("Null Ref");
                SetBlocking(false);
                SetIncombat(false);
                hasTarget = false;
                yield break;
            }

            if (_target == null)
            {
                SetBlocking(false);
                SetIncombat(false);
                hasTarget = false;
                Debug.Log("Lost Target");
                yield break;
            }



            SetIncombat(true);
            //Debug.Log("Chase");
            hasTarget = true;


            yield return new WaitForSeconds(checkTime);






            if (CheckDist() <= meleeRange)
            {
                int random = (int)Random.Range(-0.5f, 5.4f);
                transform.LookAt(_targetPos);


                if (healthScript.currentHealth <= 20f)
                {
                    SetBlocking(true);
                }

                if (random == 0 && canAttack == true && targetPos != null)
                {
                    Attack(_target);
                    RechargeAttackTimer(basicAttackCD);
                    yield return new WaitForSeconds(0.5f);
                }
                else if (random == 1 && canAttack == true && targetPos != null)
                {
                    HeavyAttack(_target);
                    RechargeAttackTimer(heavyAttackCd);
                    yield return new WaitForSeconds(0.5f);
                }
                else if (random == 2 && targetPos != null)
                {
                    SetBlocking(true);
                    healthScript.blocking = true;
                    yield return new WaitForSeconds(1.0f);
                    if (targetPos != null)
                    unitAnimator.ShieldBash(target);
                    healthScript.blocking = false;
                    SetBlocking(false);
                    yield return new WaitForSeconds(0.1f);
                }
                else if (random == 3 && targetPos != null)
                {
                    HeavyAttack(_target);
                    StartCoroutine(Strafe());
                    yield return new WaitForSeconds(0.5f);
                }
                else if (random == 4 && targetPos != null)
                {
                    Attack(_target);
                    Dodge(_targetPos.position, 5f);
                    yield return new WaitForSeconds(0.5f);
                }

                if (target.blocking == true && random <= 3f && targetPos != null)
                {
                    Rush(_targetPos.position, 5f);
                }
            }
            //  transform.rotation.SetFromToRotation(transform.position, _targetPos.position);
;
            checks++;


        }
    }

}
