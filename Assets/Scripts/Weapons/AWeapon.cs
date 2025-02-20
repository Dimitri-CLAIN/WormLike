﻿
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

public abstract class AWeapon : NetworkBehaviour
{
    #region Visual Objects

    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private ParticleSystem particles;

    #endregion

    #region Weapon Statistics

    public float blastRadius = 3f;
    public int damage = 35;

    #endregion
    

    #region Server

    /// <summary>
    /// Tells all the client to apply said angle and power to the projectile
    /// </summary>
    /// <param name="angle">Angle</param>
    /// <param name="power">Normalized power</param>
    [ClientRpc]
    public virtual void RpcFireWeapon(float angle, float power)
    {
        Vector3 vec = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle), 0);
        float p = Mathf.Lerp(20f, 1000f, power);
        rb.AddForce(vec * p);
    }


    /// <summary>
    /// Destroy the object
    /// </summary>
    [Server]
    protected virtual void DestroySelf()
    {
        Vector3 position = transform.position;
        NetworkServer.Destroy(gameObject);
        ParticleSystem particle = Instantiate(particles, position, Quaternion.identity);//Instantiate(particles.gameObject, this.transform.position, particles.gameObject.transform.rotation);
        NetworkServer.Spawn(particle.gameObject);
        
        // Dmg logic
        Collider[] hits = Physics.OverlapSphere(position, blastRadius);
        List<int> hitsGameObjectsID = new List<int>();
        for (int i = 0; i < hits.Length; i++)
        {
            Collider objectHit = hits[i];
            // if object has 2 colliders, it will figure twice in the OverlapSphere, this assure the Game Object
            // is counted only once by storing its gameObject's Instance ID and check if it has already been passed on
            if (hitsGameObjectsID.Contains(objectHit.gameObject.GetInstanceID()))
                continue;
            hitsGameObjectsID.Add(objectHit.gameObject.GetInstanceID());
            
            if (objectHit.TryGetComponent<IDamageable>(out IDamageable target))
            {
                target.DealDamage(damage);
            }
        }
    }

    
    #endregion
}
