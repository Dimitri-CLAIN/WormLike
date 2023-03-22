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
    protected int maxHealth = 100;
    [SerializeField]
    [SyncVar(hook = nameof(HandleHealthChange))]
    protected int health;

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


    /// <summary>
    /// Add value of health to the map component
    /// </summary>
    /// <param name="value">Amount of health added</param>
    [ServerCallback]
    public virtual void Add(int value)
    {
        if (value > 0)
            health = Mathf.Min(health + value, maxHealth);
    }


    /// <summary>
    /// Subtract value of health to the map component
    /// </summary>
    /// <param name="value">Amount of health subtracted</param>
    [ServerCallback]
    public virtual void Remove(int value)
    {
        if (value <= 0) return;
        
        health -= value;
    }

    #endregion

    #region Client

    [Client]
    protected virtual void HandleHealthChange(int oldHealth, int newHealth)
    {
        // TODO tween health lost
        health = newHealth;

        if (health <= 0)
        {
            OnDeath?.Invoke();
            NetworkServer.Destroy(this.gameObject);
        }
        if (healthbar != null)
        {
            float normalizedValue = Mathf.InverseLerp(0, maxHealth, health);
            healthbar.fillAmount = normalizedValue;
        }
    }

#endregion
    
    
}
