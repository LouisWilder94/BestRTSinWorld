using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

//[RequireComponent(typeof(Animator))]
public class UnitAnimator : NetworkBehaviour {

    // [HideInInspector]
    public Unit unitScript;
    public Animator animator;
    public NavMeshAgent navAgent;

    [Header("Melee Attack 1")]
    public string[] attack1Anims;
    public float[] attack1AnimHitTimes;

    [Header("Melee Attack Heavy")]
    public string[] attack2Anims;
    public float[] attack2AnimHitTimes;
    public float[] attack2SecondaryHitTimes;

    [Header("While Blocking Attacks")]
    public string[] blockAttackAnims;
    public float[] blockAnimHitTimes;

    [Header("Light Ranged Attack")]
    public string[] lightRangedAttacks;
    public float[] lightRangedAttacksShootPoint;

    [Header("Heavy Ranged Attack")]
    public string[] heavyRangedAttacks;
    public float[] heavyRangedAttackShootPoint;

    public void randomAttack1(UnitHealth target)
    {
        if (unitScript.blocking == true)
        {
            BlockAttack(target);
            return;
        }

        int RandomNum = (int)Random.Range(0, attack1Anims.Length);
        animator.SetTrigger(attack1Anims[RandomNum]);
        target.TakeDamageWDelayed(unitScript.basicAttackDamage, attack1AnimHitTimes[RandomNum]);
    }

    public void randomAttack2(UnitHealth target)
    {
        if (unitScript.blocking == true)
        {
            BlockAttack(target);
            return;
        }

        int RandomNum = (int)Random.Range(0, attack2Anims.Length);
        animator.SetTrigger(attack2Anims[RandomNum]);
        target.TakeDamageWDelayed(unitScript.basicAttackDamage, attack2AnimHitTimes[RandomNum]);
        target.TakeDamageWDelayedWKnockback(unitScript.basicAttackDamage, attack2SecondaryHitTimes[RandomNum], transform.position, (unitScript.heavyAttackDamage / 2.5f));
    }

    public void BlockAttack(UnitHealth target)
    {
        int RandomNum = (int)Random.Range(0, blockAttackAnims.Length);
        animator.SetTrigger(blockAttackAnims[RandomNum]);
        target.TakeDamageWDelayed(unitScript.basicAttackDamage, blockAnimHitTimes[RandomNum]);
    }

    public void randomShootAttackLight(GameObject prefab, Transform targetPos, UnitHealth target, Transform shootPosition)
    {
        int RandomNum = (int)Random.Range(0, heavyRangedAttacks.Length);
        animator.SetTrigger(lightRangedAttacks[RandomNum]);
        StartCoroutine(WaitAndShoot(prefab, targetPos, target, shootPosition, lightRangedAttacksShootPoint[RandomNum]));
    }

    public void randomShootAttackHeavy(GameObject prefab, Transform targetPos, UnitHealth target, Transform shootPosition)
    {
        int RandomNum = (int)Random.Range(0, heavyRangedAttacks.Length);
        animator.SetTrigger(heavyRangedAttacks[RandomNum]);
        StartCoroutine(WaitAndShoot(prefab, targetPos, target, shootPosition, heavyRangedAttackShootPoint[RandomNum]));
    }

    public void DodgeBack()
    {
        animator.SetTrigger("DodgeBack");
    }

    public IEnumerator WaitAndShoot(GameObject prefab, Transform targetPos, UnitHealth target, Transform shootPosition, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Fire(prefab, targetPos, target, shootPosition);
    }

    public void Fire(GameObject prefab, Transform targetPos, UnitHealth target, Transform shootPosition)
    {
        GameObject projectile = (GameObject)Instantiate(prefab, shootPosition.position, Quaternion.identity);
        Projectile projscript = projectile.GetComponent<Projectile>();
        projscript.target = target;
        projscript.targetTrans = targetPos;
    }

    //[Header("Animation States")]

    public string SetBoolTrue
    {
        set
        {
            animator.SetBool(value, true);
        }
    }

    public string SetBoolFalse
    {
        set
        {
            animator.SetBool(value, false);
        }
    }

    public bool moving;

    public void SetMoving(bool value)
    {
        moving = value;
        animator.SetBool("Move", value);
    }

    public void playAnimation(string stateName, int layer)
    {
        animator.Play(stateName, layer);
    }

    public IEnumerator checkMoving()
    {
        while (true)
        {

            yield return new WaitForSeconds(0.3f);
        }
    }

    private void FixedUpdate()
    {
        // animator.SetFloat("Speed", navAgent.velocity.magnitude);
        // Debug.Log(navAgent.velocity.magnitude);

        if (navAgent.velocity.sqrMagnitude >= 0.2f)
            moving = true;
        else
        {
            moving = false;
        }
    }

    private void Awake()
    {
    }

    private void Start()
    {
        unitScript = GetComponent<Unit>();
        StartCoroutine(checkMoving());
    }

    public void SetBool(string _bool, bool value)
    {
        animator.SetBool(_bool, value);
    }
}
