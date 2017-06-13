using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

//[RequireComponent(typeof(Animator))]
public class UnitAnimator : NetworkBehaviour {

   // [HideInInspector]
    public Animator animator;
    public NavMeshAgent navAgent;

    [Header("Attack 1")]
    public string[] attack1Anims;

    public void randomAttack1()
    {
        animator.SetTrigger(attack1Anims[Random.Range(0, attack1Anims.Length)]);
    }

    [Header("Attack 2")]
    public string[] attack2Anims;

    public void randomAttack2()
    {
        animator.SetTrigger(attack2Anims[Random.Range(0, attack2Anims.Length)]);
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
        StartCoroutine(checkMoving());
    }

    public void SetBool(string _bool, bool value)
    {
        animator.SetBool(_bool, value);
    }
}
