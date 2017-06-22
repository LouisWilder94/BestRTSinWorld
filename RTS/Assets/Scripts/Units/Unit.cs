using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.AI;
using System.Linq;

public class Unit : NetworkBehaviour
{
    [Header("Attributes")]
    public float basicAttackDamage = 10f;
    public float basicAttackCD = 1f;

    public float heavyAttackDamage = 20f;
    public float heavyAttackCd = 2f;

    public float meleeRange = 6f;
    public float rangedRange = 20f;

    public int unitCost;
    public float buildTime;

    [HideInInspector]
    public float startSpeed;
    [HideInInspector]
    public float startAcceleration;
    [HideInInspector]
    public float StartAngularSpeed;


    [Space]


    [Header("Startup")]
    public NavMeshAgent navAgent;
    public UnitAnimator unitAnimator;
    public UnitHealth healthScript;


    [Space]

    [Header("Behaviors")]
    public bool dodge = true;
    public float dodgePower;

    public bool strafe = true;
    public float strafeDist = 4f;
    public float StrafePower = 20f;

    public float knockbackRotDisableTime = 0.4f;
    public float navmeshDestinationReachDist = 2f;
    public bool canMove = true;

    [Space]

    [Header("Enabled Attack Patterns")]
    public bool demonMelee;
    public bool demonRanged;
    public bool angleSpearman;


    [Space]


    [Header("Ranged Behavior")]
    public GameObject LightProjectile;
    public Transform shootPosition;

    public bool usesHeavyProjectile;
    public GameObject HeavyProjectile;

    public float rangedSkirmishDist = 20f;
    public float dodgeBackDist = 5f;
    public bool useMuzzle = false;
    public float muzzleTime = 0.3f;

    public float lightProjectileCooldown = 0.5f;
    public float heavyProjectileCooldown = 2f;

    [Space]

    [Header("Detection")]
    [HideInInspector]
    public BoxCollider detectCollider;
    public float detectDistance;
    public const float StartCheckTime = 1.3f;
    public float checkTime = StartCheckTime;



    //private/hidden vars
    [HideInInspector]
    public int playerOwnership;

    private Collider unitCollider;

    private bool hasTarget = false;

    private bool isInRange = false;

    [HideInInspector]
    public UnitHealth target;

    [HideInInspector]
    public Transform targetPos;

    [HideInInspector]
    public bool incombat;





    //Setting navmesh values
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
    //end navmesh values




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
        checkTime += Random.Range(0, 0.2f);

        navAgent = GetComponent<NavMeshAgent>();
        unitAnimator = gameObject.GetComponent<UnitAnimator>();
       // CreateDetectionCollider();

        InvokeRepeating("AgroUpdateCheck", 0, 2);
        CheckAgro();

        unitCollider = gameObject.GetComponent<Collider>();
      //  UnitHealth target = FunctionsHelper.GetNearestUnit(transform.position, healthScript);
     //   ChaseTarget(target, target.transform);
    }

    public IEnumerator lookatAndDisableRotation(Vector3 target, float disableTime)
    {
        angularSpeed = 0;
        //transform.Rotate()
       // transform.LookAt(target);
        yield return new WaitForSeconds(disableTime);
        angularSpeed = StartAngularSpeed;
    }
    //end setting states



    //Movement handeling and navigation
    public void Move(Vector3 position)
    {
        navAgent.SetDestination(position);
    }

    public float CheckDist()
    {
        return (transform.position - navAgent.destination).magnitude;
    }

    public float CheckDistFromtarget(Transform _Target)
    {
            return (transform.position - _Target.position).magnitude;
    }

    public void Dodge(Vector3 objToAvoid, float power)
    {
        if (dodge == false)
            return;

        Vector3 currentPos = transform.position;
        Vector3 angle = (currentPos - objToAvoid).normalized;
        Vector3 dodgePos = angle * power;
        //navAgent.SetDestination(dodgePos);
        navAgent.velocity = dodgePos;
        StartCoroutine(lookatAndDisableRotation(objToAvoid, knockbackRotDisableTime));
    }
    
    public void Knockback(Vector3 source, float power)
    {
        Vector3 dodgeAngle = (transform.position - source).normalized * power;
        navAgent.velocity = dodgeAngle;
        StartCoroutine(lookatAndDisableRotation(source, knockbackRotDisableTime));
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
        int checks = 0;

        if (strafe == false)
            yield break;
        canMove = false;

        CompleteDest = true;
        Vector3 pos = GetRandomStrafePos(strafeDist);
        navAgent.SetDestination(pos);
        while (true)
        {
            checks++;
            navAgent.SetDestination(pos);

            if (CheckDist() <= navmeshDestinationReachDist | checks >= 10)
            {
                angularSpeed = StartAngularSpeed;
                maxSpeed = startSpeed;
                Acceleration = startAcceleration;
                canMove = true;
                yield break;
            }


            angularSpeed = 0f;
            maxSpeed = StrafePower;
            Acceleration = StrafePower * 20;



            yield return new WaitForSeconds(checkTime);
        }

    }

    public IEnumerator DodgeBack(Vector3 fromPosition)
    {
        int checks = 0;

        if (dodge == false)
            yield break;

        transform.LookAt(fromPosition);
        canMove = false;

        CompleteDest = true;
        Vector3 pos = GetPositionFromLocal(Vector3.back, dodgeBackDist);
        navAgent.SetDestination(pos);
        unitAnimator.DodgeBack();
        while (true)
        {
            checks++;
            navAgent.SetDestination(pos);

            if (CheckDist() <= navmeshDestinationReachDist | checks >= 20)
            {
                yield return new WaitForSeconds(0.1f);
                angularSpeed = StartAngularSpeed;
                maxSpeed = startSpeed;
                Acceleration = startAcceleration;
                canMove = true;
                yield break;
            }


            angularSpeed = 0f;
            maxSpeed = StrafePower;
            Acceleration = StrafePower * 20;



            yield return new WaitForSeconds(0.1f);
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

    public Vector3 GetRandomStrafePos(float distance)
    {
        Vector3 dist;
        float random = Random.Range(0f, 2f);
        if (random <= 1f)
        {
            dist = Vector3.left;
        }
        else
        {
            dist = Vector3.right;
        }

        return GetPositionFromLocal(dist, distance);
    }

    public void RotateToDirection(Transform _target)
    {
        Vector3 difference = _target.position - transform.position;
        difference.Normalize();

        float rotZ = Mathf.Atan2(difference.x, difference.z) * Mathf.Rad2Deg; //find the angle in degrees
        Quaternion lookRotation = Quaternion.LerpUnclamped(transform.rotation, Quaternion.Euler(0, rotZ, 0), 1f);
        transform.rotation = lookRotation;
    }

    IEnumerator rotate(Transform _target)
    {
        while (true)
        {
            angularSpeed = 0;
            RotateToDirection(_target);
            if (hasTarget == false)
            {
                yield break;
                angularSpeed = StartAngularSpeed;
            }


            yield return new WaitForEndOfFrame();
        }
    }

    public Vector3 GetPositionFromLocal(Vector3 directionFromLocal, float distance)
    {
        Vector3 rot = transform.rotation.eulerAngles;
        Vector3 vec = Quaternion.Euler(rot.x, rot.y, rot.z) * (directionFromLocal) * distance;
        Vector3 pos = transform.position + vec;
        return pos;
    }

    public Vector3 GetPositionDistanceFromPoint(Vector3 position, float distance)
    {
        Vector3 angle = (transform.position - position).normalized;
        return position + angle * distance;
    }
    //end movement handling and navigation



    //Attacks
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

    public IEnumerator OverWhelm(UnitHealth target)
    {
        if (canAttack == true)
        {
            try
            {
                canAttack = false;
                SetBlocking(false);
                transform.LookAt(target.transform);
                unitAnimator.randomAttack1(target);
            }
            catch
            {
                canAttack = true;
                hasTarget = false;
                yield break;
            }
            yield return new WaitForSeconds(0.5f);
            try
            {
                canAttack = false;
                SetBlocking(false);
                transform.LookAt(target.transform);
                unitAnimator.randomAttack1(target);
            }
            catch
            {
                canAttack = true;
                hasTarget = false;
                yield break;
            }
            yield return new WaitForSeconds(0.4f);
            try
            {
                canAttack = false;
                SetBlocking(false);
                transform.LookAt(target.transform);
                unitAnimator.randomAttack1(target);
            }
            catch
            {
                canAttack = true;
                hasTarget = false;
                yield break;
            }
            yield return new WaitForSeconds(0.3f);
            try
            {
                canAttack = false;
                SetBlocking(false);
                transform.LookAt(target.transform);
                unitAnimator.randomAttack1(target);
            }
            catch
            {
                canAttack = true;
                hasTarget = false;
                yield break;
            }
            yield return new WaitForSeconds(0.25f);
            try
            {
                canAttack = false;
                SetBlocking(false);
                transform.LookAt(target.transform);
                unitAnimator.randomAttack1(target);
            }
            catch
            {
                canAttack = true;
                hasTarget = false;
                yield break;
            }
            yield return new WaitForSeconds(0.25f);
            canAttack = true;
        }
        else
        {
            yield break;
        }
    }

    public void HeavyAttack(UnitHealth target)
    {
        if (canAttack == false)
            return;

        unitAnimator.randomAttack2(target);
    }
    //end Attacks


     
      //ranged attacks
    public void CreateMuzzleEffect(GameObject prefab)
    {
        GameObject FX = (GameObject)Instantiate(prefab, shootPosition.position, shootPosition.rotation);
        Destroy(FX, muzzleTime);
    }
    //End ranged attacks



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

        if (demonMelee == true)
        {
            StartCoroutine(MeleeChase(target, targetPos));
        }
        else if(demonRanged == true)
        {
            StartCoroutine(RangedChase(target, targetPos));
        }

    }

    public void OnTriggerEnter(Collider other)
    {
             if (hasTarget)
                 return;
        
             if (other.GetComponent<UnitHealth>() && other.tag != gameObject.tag) //&& other.tag != gameObject.tag)
             {
            //    Debug.Log(other);
            //   StartCoroutine(Chase(other.GetComponent<UnitHealth>(), other.transform));
            }
    }


    private int agroChecks = 0;
    public void CheckAgro()
    {
        if (hasTarget)
            return;
        agroChecks++;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectDistance, GameManager.instance.unitLayermask);

        List<Collider> possibleTargets = hitColliders.ToList<Collider>();
        possibleTargets.Remove(unitCollider);

        int random = Random.Range(0, possibleTargets.Count);

        try
        {
            if (possibleTargets[random].tag != this.tag)
            {
                UnitHealth _target = possibleTargets[random].GetComponent<UnitHealth>();
                ChaseTarget(_target, _target.transform);
                agroChecks = 0;
            }
            else if (agroChecks <= 2)
            {
                CheckAgro();
            }
        }
        catch
        {
            return;
        }


    }
    //end Target Handling


    //Combat Behavior
    float dist;
    int checks;
    public IEnumerator MeleeChase(UnitHealth _target, Transform _targetPos)
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
            CheckAgro();
        }


        target = _target;
        targetPos = _targetPos;
     //   StartCoroutine(rotate(_targetPos));
        checks = 0;

        while (true)
        {
            try
            {
                    navAgent.destination = _targetPos.position;
            }
            catch
            {
              //  Debug.LogError("Null Ref");
                SetBlocking(false);
                SetIncombat(false);
                hasTarget = false;
                CheckAgro();
                yield break;
            }

            if (_target == null)
            {
                SetBlocking(false);
                SetIncombat(false);
                hasTarget = false;
                Debug.Log("Lost Target");
                CheckAgro();
                yield break;
            }


        //    if (checks >= 30)
        //    {
        //        hasTarget = false;
        //        CheckAgro();
        //        yield break;
        //    }



            SetIncombat(true);
            //Debug.Log("Chase");
            hasTarget = true;


            yield return new WaitForSeconds(checkTime);




            //start behavior demon melee
                if (CheckDist() <= meleeRange)
                {
                    int random = (int)Random.Range(-0.5f, 7.4f);
                    transform.LookAt(_targetPos);


                    if (healthScript.currentHealth <= 20f)
                    {
                        int rand = (int)Random.Range(0, 1);

                        if (rand == 0)
                            SetBlocking(true);
                        if (rand == 1)
                            StartCoroutine(OverWhelm(target));
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
                            unitAnimator.BlockAttack(target);
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
                    else if (target.blocking == true && random == 5 && targetPos != null && canAttack == true)
                    {
                        try
                        {
                            Rush(_targetPos.position, 5f);
                        }
                        catch
                        {
                            CheckAgro();
                            yield break;
                        }
                       StartCoroutine(OverWhelm(target));
                        yield return new WaitForSeconds(0.4f);
                        try
                        {
                            Rush(_targetPos.position, 5f);
                        }
                        catch
                        {
                            CheckAgro();
                            yield break;
                        }
                        yield return new WaitForSeconds(0.4f);
                        try
                        {
                            Rush(_targetPos.position, 5f);
                        }
                        catch
                        {
                            CheckAgro();
                            yield break;
                        }
                    }
                    else
                    {
                        Attack(_target);
                        RechargeAttackTimer(basicAttackCD);
                        yield return new WaitForSeconds(0.5f);
                    }
                }
            checks++;


        }
    }

    public IEnumerator RangedChase(UnitHealth _target, Transform _targetPos)
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
            CheckAgro();
        }


        target = _target;
        targetPos = _targetPos;
        //   StartCoroutine(rotate(_targetPos));
        checks = 0;
        SetIncombat(true);

        try
        {
            navAgent.destination = GetPositionDistanceFromPoint(_targetPos.position, rangedSkirmishDist);
        }
        catch
        {
            SetIncombat(false);
            hasTarget = false;
            CheckAgro();
            yield break;
        }

        while (true)
        {



            if (_target == null)
            {
               SetIncombat(false);
                hasTarget = false;
                Debug.Log("Lost Target");
                CheckAgro();
                yield break;
            }


            //    if (checks >= 30)
            //    {
            //        hasTarget = false;
            //        CheckAgro();
            //        yield break;
            //    }



         //   SetIncombat(true);
            hasTarget = true;


            yield return new WaitForSeconds(checkTime);
                try
                {
                    dist = CheckDistFromtarget(_targetPos);
                }
                catch
                {
                    hasTarget = false;
                    CheckAgro();
                    yield break;
                }

                int random = (int)Random.Range(0, 5.4f);


            //Debug.Log(dist);
            if (dist <= meleeRange)
            {
                if(random == 0 | random == 1)
                {
                    Attack(target);
                }
                else if(random == 2 | random == 3)
                {
                    StartCoroutine(DodgeBack(_targetPos.position));
                }
                else if(random == 4 | random == 5)
                {
                    navAgent.destination = GetPositionDistanceFromPoint(_targetPos.position, rangedSkirmishDist);
                }
            }
            else if (dist <= rangedRange)
            {
                if (random == 0 && canAttack == true)
                {
                    transform.LookAt(_targetPos);
                   // DodgeBack(_targetPos.position);
                    unitAnimator.randomShootAttackLight(LightProjectile, targetPos, target, shootPosition);
                    RechargeAttackTimer(lightProjectileCooldown);
                    yield return new WaitForSeconds(lightProjectileCooldown);
                }
                else if (random == 1 && canMove == true)
                {
                    navAgent.destination = GetPositionDistanceFromPoint(_targetPos.position, rangedSkirmishDist);
                }
                else if (random == 2 && canMove == true)
                {
                    navAgent.destination = GetPositionDistanceFromPoint(_targetPos.position, rangedSkirmishDist);
                }
                else if (random == 3 && canMove == true)
                {
                    StartCoroutine(Strafe());
                }
                else if (random == 4 && canAttack == true)
                {
                    transform.LookAt(_targetPos);
                    // DodgeBack(_targetPos.position);
                    unitAnimator.randomShootAttackLight(LightProjectile, targetPos, target, shootPosition);
                    RechargeAttackTimer(lightProjectileCooldown);
                    yield return new WaitForSeconds(lightProjectileCooldown);
                    transform.LookAt(_targetPos);
                    // DodgeBack(_targetPos.position);
                    unitAnimator.randomShootAttackLight(LightProjectile, targetPos, target, shootPosition);
                    RechargeAttackTimer(lightProjectileCooldown);
                }
                else if (random == 5 && canAttack == true)
                {
                    Debug.Log("Heavy Atk");
                    transform.LookAt(_targetPos);
                    // DodgeBack(_targetPos.position);
                    unitAnimator.randomShootAttackHeavy(HeavyProjectile, targetPos, target, shootPosition);
                    RechargeAttackTimer(heavyProjectileCooldown);
                    yield return new WaitForSeconds(heavyProjectileCooldown);
                }

            }
            checks++;

        }
    }
    //End Combat Behavior

        //TODO:Jump back and thrust spear attack

    //Testing
    private void OnDrawGizmos()
    {
        //  Gizmos.DrawWireSphere(GetPositionFromLocal(Vector3.left, strafeDist), 1f);
        //    Gizmos.DrawWireSphere(GetPositionFromLocal(Vector3.right, strafeDist), 1f);
        //   Gizmos.DrawWireSphere(GetPositionFromLocal(Vector3.forward, strafeDist), 1f);
        //   Gizmos.DrawWireSphere(GetPositionFromLocal(Vector3.back, strafeDist), 1f);
        //  Gizmos.DrawWireSphere(GetPositionDistanceFromPoint(FunctionsHelper.GetCursorPosition(0), rangedRange), 5f);

     //   Gizmos.DrawWireSphere(GetPositionDistanceFromPoint(targetPos.position, dodgeBackDist), 1f);
    }

    public void AgroUpdateCheck()
    {
    //    Debug.Log("CheckingTargets");
        CheckAgro();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(DodgeBack(FunctionsHelper.GetCursorPosition(0)));
        }
    }

    //TODO:Target Facing, Optimize agro check with high unit numbers
}
