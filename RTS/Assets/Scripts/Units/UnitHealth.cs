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

    [Space]

    [Header("Startup")]
    [HideInInspector]
    public UnitAnimator animator;

    public bool InstantiateSliderOnStartup = true;
    private Slider slider;
    public GameObject SliderPrefab;
    public Transform SliderPosition;

    //[SyncVar(hook = "OnChangeHealth")]

    void Start () {
        currentHealth = maxHealth;
        animator = GetComponent<UnitAnimator>();
        if (animator == null)
            Debug.LogError("No unit animator attatched to health script");

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
	
	void Update () {


        //TODO:optimize
        //SliderPosition.LookAt(Camera.main.transform);
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

        if (!blocking)
        {
            currentHealth -= damage;
            slider.value -= damage;
        }
        else
        {
            currentHealth -= damage * blockDmgRatio;
            slider.value -= damage * blockDmgRatio;
        }

        if (currentHealth <= 0)
        {
            Die();
        }

        //healthBar.sizeDelta = new Vector2(currentHealth, healthBar.sizeDelta.y);
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
        Debug.Log("Dead!");
        animator.SetBoolTrue = "Die";
        animator.playAnimation("Die", 0);
        gameObject.tag = null;
        Invunerable = true;
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
