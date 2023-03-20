using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class IDamageable : MonoBehaviour
{
    [SerializeField]
    private Health healthComponent = null;


    /// <summary>
    /// Identify if a Game Object is damageable, then call this function to effectively deal damages to the Health.
    /// </summary>
    /// <param name="damages">Amount of damage dealt</param>
    public virtual void DealDamage(int damages)
    {
        if (healthComponent == null)
            healthComponent = GetComponent<Health>();
        healthComponent.Remove(damages);
    }
}
