using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class UnitHealth : NetworkBehaviour {

    [SerializeField]
    const int maxHealth = 100;

    //[SyncVar(hook = "OnChangeHealth")]
    [SyncVar]
    public int currentHealth = maxHealth;
    //public RectTransform healthBar;

    bool blocking = false;

	void Start () {
        currentHealth = maxHealth;
	}
	
	void Update () {
		
	}

    public void TakeDamage(int damage)
    {
        if (!isServer)
        {
            return;
        }
        if (!blocking)
        {
            currentHealth -= damage;
        }

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("Dead!");
            NetworkServer.Destroy(gameObject);
        }

        //healthBar.sizeDelta = new Vector2(currentHealth, healthBar.sizeDelta.y);
    }

    //void OnChangeHealth(int health)
    //{
    //    healthBar.sizeDelta = new Vector2(health, healthBar.sizeDelta.y);
    //}
}
