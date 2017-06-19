using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    [HideInInspector]
    public UnitHealth target;
    public Transform targetTrans;
    public float fizzleTime;
    public float directDamage;
    public float indirectDamage;
    public float speed;
    public float turnSpeed;
    public float sinCurvePower;
    public float sinCurveChangeRate;
    public float explosionKnockback;

    public float hitTargetDist;

    public bool explosive;
    public float explosionRadius;
    public GameObject explosionPrefab;
    public float explosionPrefabFizzleTime;
    
    public void updateTarget(UnitHealth _target, Transform _targetTrans)
    {
        target = _target;
        targetTrans = _targetTrans;
    }

    public void Update()
    {
        float randomSin = Mathf.Sin(Time.timeSinceLevelLoad * sinCurveChangeRate);

        try
        {
            Quaternion rotation = Quaternion.LookRotation((targetTrans.position + Vector3.up * 3) - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * turnSpeed * randomSin);
        }
        catch
        {
            fizzle();
            return;
        }





        Vector3 ajustedDirection = new Vector3(0, Mathf.Sin(Time.timeSinceLevelLoad) * sinCurvePower, 0);
        transform.position += transform.forward * speed * Time.deltaTime;

        if ((transform.position - (targetTrans.position + Vector3.up * 3)).sqrMagnitude <= hitTargetDist)
        {
            fizzle();

            if (explosive == false)
                DealDirectDamage();
            else
            {
                DealDirectDamage();
                Explode();
            }
        }
    }

    public void fizzle()
    {
        Destroy(gameObject);
        GameObject FX = (GameObject)Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(FX, explosionPrefabFizzleTime);
    }

    public void Explode()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);

        for (int i = 0; i < hitColliders.Length; i++)
        {
           UnitHealth unit = hitColliders[i].GetComponent<UnitHealth>();
            try
            {
                unit.TakeDamageWDelayedWKnockback(indirectDamage, 0.1f, transform.position, explosionKnockback);
            }
            catch
            {

            }

        }
    }

    public void DealDirectDamage()
    {
        try
        {
            target.TakeDamage(directDamage);
        }
        catch
        {
            return;
        }

    }
}
