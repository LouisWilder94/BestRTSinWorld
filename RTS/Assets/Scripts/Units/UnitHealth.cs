using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[RequireComponent(typeof(UnitAnimator))]
public class UnitHealth : NetworkBehaviour {


    [Header("Attributes")]
    [SerializeField]
    public float maxHealth = 100;
    [SyncVar]
    public float currentHealth;
    public float blockDmgRatio = 0.5f;
    public float healthRegenRate = 1f;
    public float healthRegenAmount = 1f;
    //50% of damage will be applied while blocking

    [Space]

    [Header("RuntimeVariables")]
    [SyncVar]
    public bool blocking = false;
    public bool Invunerable = false;
    public bool incomingAttak = false;

    [Space]

    [Header("Startup")]
    [HideInInspector]
    public UnitAnimator animator;
    public Unit unitScript;

    public bool InstantiateSliderOnStartup = true;
    private Slider slider;
    public GameObject SliderPrefab;
    public Transform SliderPosition;

    [Header("Blood Effects")]
    public GameObject bloodSpurtPrefab;
    public GameObject sparksPrefab;

    //[SyncVar(hook = "OnChangeHealth")]

    void Start () {
        currentHealth = maxHealth;
        animator = GetComponent<UnitAnimator>();
        if (animator == null)
            Debug.LogError("No unit animator attatched to health script");

        unitScript = GetComponent<Unit>();

        StartCoroutine(healthRegen());

        if (InstantiateSliderOnStartup == true)
        {
            GameObject SliderClone = (GameObject)Instantiate(SliderPrefab, SliderPosition.position, SliderPosition.rotation, SliderPosition);
            slider = SliderClone.GetComponentInChildren<Slider>();
            slider.maxValue = maxHealth;
            slider.value = currentHealth;
            SliderPrefab = SliderClone;
        }
	}
	
    public void CreateAndDestroyEffect(GameObject FX)
    {
        GameObject instance = (GameObject)Instantiate(FX, transform.position, Quaternion.identity);
        Destroy(instance, 1f);
    }

    public void TakeDamage(float damage)
    {
        if (!isServer)
        {
            return;
        }
        if(Invunerable == true)
        {
            return;
        }

        if (blocking == false)
        {
            currentHealth -= damage;
            slider.value -= damage;
            CreateAndDestroyEffect(bloodSpurtPrefab);
        }
        else
        {
            CreateAndDestroyEffect(sparksPrefab);
            currentHealth -= (damage * blockDmgRatio);
            slider.value -= (damage * blockDmgRatio);
        }

        if (currentHealth <= 0)
        {
            Die();
        }

        //healthBar.sizeDelta = new Vector2(currentHealth, healthBar.sizeDelta.y);
    }

    public void TakeDamageWDelayedWKnockback(float damage, float hitTime, Vector3 knockbackSource, float knockbackPower)
    {
        try
        {
            StartCoroutine(TakeDamageWithDelayIEnumeratorWKnockback(damage, hitTime, knockbackSource, knockbackPower));
        }
        catch
        {
            return;
        }

    }

    public void TakeDamageWDelayed(float damage, float hitTime)
    {
        try
        {
            StartCoroutine(TakeDamageWithDelayIEnumerator(damage, hitTime));
        }
        catch
        {
            return;
        }

    }

    public IEnumerator TakeDamageWithDelayIEnumeratorWKnockback(float damage, float hitTime, Vector3 knockbackSource, float knockbackPower)
    {
        yield return new WaitForSeconds(hitTime);

        if (!isServer)
        {
            yield break;
        }
        if (Invunerable == true)
        {
            yield break;
        }

        if (blocking == false)
        {
            currentHealth -= damage;
            slider.value -= damage;
            CreateAndDestroyEffect(bloodSpurtPrefab);
        }
        else
        {
            CreateAndDestroyEffect(sparksPrefab);
            currentHealth -= (damage * blockDmgRatio);
            slider.value -= (damage * blockDmgRatio);
        }

        unitScript.Knockback(knockbackSource, knockbackPower);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public IEnumerator TakeDamageWithDelayIEnumerator(float damage, float hitTime)
    {
        yield return new WaitForSeconds(hitTime);

        if (Invunerable == true)
        {
            yield break;
        }

        if (blocking == false)
        {
            currentHealth -= damage;
            slider.value -= damage;
            CreateAndDestroyEffect(bloodSpurtPrefab);
        }
        else
        {
            CreateAndDestroyEffect(sparksPrefab);
            currentHealth -= (damage * blockDmgRatio);
            slider.value -= (damage * blockDmgRatio);
        }
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void gainHealth(float amount)
    {
        if (!isServer)
        {
            return;
        }
        if (currentHealth >= maxHealth)
            return;

        currentHealth += amount;
        slider.value += amount;
    }

    public void Die()
    {
        currentHealth = 0;
        slider.value = 0;
     //   Debug.Log("Dead!");
        animator.SetBoolTrue = "Die";
        animator.playAnimation("Die", 0);
        Invunerable = true;
        CreateAndDestroyEffect(bloodSpurtPrefab);
        CreateAndDestroyEffect(bloodSpurtPrefab);
        CreateAndDestroyEffect(bloodSpurtPrefab);
        CreateAndDestroyEffect(bloodSpurtPrefab);
        CreateAndDestroyEffect(bloodSpurtPrefab);
        CreateAndDestroyEffect(bloodSpurtPrefab);
        StartCoroutine(WaitAndDie());
    }

    public IEnumerator WaitAndDie()
    {
        yield return new WaitForSeconds(1f);
        NetworkServer.Destroy(gameObject);
        yield break;
    }


    public IEnumerator healthRegen()
    {
        while(true)
        {
            gainHealth(healthRegenAmount);
            yield return new WaitForSeconds(healthRegenAmount);
        }
    }
}
