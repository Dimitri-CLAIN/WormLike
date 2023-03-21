using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class Health : NetworkBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private int maxHealth = 100;
    [SerializeField]
    [SyncVar(hook = nameof(HandleHealthChange))]
    private int health;

    public event Action OnDeath;

    #region Visuals

    [SerializeField]
    private Image healthbar;

    #endregion

    #region Server

    
    [ServerCallback]
    public override void OnStartServer()
    {
        health = maxHealth;
    }

    
    [ServerCallback]
    public void Add(int value)
    {
        if (value > 0)
            health = Mathf.Min(health + value, maxHealth);
    }


    [ServerCallback]
    public void Remove(int value)
    {
        if (value <= 0) return;
        
        health -= value;
        if (health <= 0)
            OnDeath?.Invoke();
        
    }

    #endregion

    #region Client

    [Client]
    private void HandleHealthChange(int oldHealth, int newHealth)
    {
        // TODO tween health lost
        health = newHealth;
        if (healthbar == null) return;

        float normalizedValue = Mathf.InverseLerp(0, maxHealth, health);
        healthbar.fillAmount = normalizedValue;
    }

#endregion
    
    
}
