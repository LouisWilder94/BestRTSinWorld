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

    [Header("Attack 1")]
    public string[] attack1Anims;
    public float[] attack1AnimHitTimes;

    public void randomAttack1(UnitHealth target)
    {
        if (unitScript.blocking == true)
        {
            ShieldBash(target);
            return;
        }

        int RandomNum = (int)Random.Range(0, attack1Anims.Length);
        animator.SetTrigger(attack1Anims[RandomNum]);
        target.TakeDamageWDelayed(unitScript.basicAttackDamage, attack1AnimHitTimes[RandomNum], transform.position, (unitScript.basicAttackDamage / 3));
    }

    [Header("Attack 2")]
    public string[] attack2Anims;
    public float[] attack2AnimHitTimes;
    public float[] attack2SecondaryHitTimes;

    public float blockAnimHitTime;

    public void randomAttack2(UnitHealth target)
    {
        if (unitScript.blocking == true)
        {
            ShieldBash(target);
            return;
        }

        int RandomNum = (int)Random.Range(0, attack2Anims.Length);
        animator.SetTrigger(attack2Anims[RandomNum]);
        target.TakeDamageWDelayed(unitScript.basicAttackDamage, attack2AnimHitTimes[RandomNum], transform.position, (unitScript.basicAttackDamage / 2.5f));
        target.TakeDamageWDelayed(unitScript.basicAttackDamage, attack2SecondaryHitTimes[RandomNum], transform.position, (unitScript.heavyAttackDamage / 2.5f));
    }

    public void ShieldBash(UnitHealth target)
    {
        int RandomNum = (int)Random.Range(0, attack1Anims.Length);
        animator.SetTrigger(attack1Anims[RandomNum]);
        target.TakeDamageWDelayed(unitScript.basicAttackDamage, blockAnimHitTime, transform.position, (unitScript.basicAttackDamage / 3));
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

        bool moving = false;
        if (navAgent.velocity.sqrMagnitude >= 0.2f)
            moving = true;
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
