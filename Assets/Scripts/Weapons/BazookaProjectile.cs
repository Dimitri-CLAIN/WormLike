using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class BazookaProjectile : NetworkBehaviour
{
    public Rigidbody rb;

    #region Server

    /// <summary>
    /// Tells all the client to apply said angle and power to the projectile
    /// </summary>
    /// <param name="angle">Angle</param>
    /// <param name="power">Normalized power</param>
    [ClientRpc]
    public void RpcAddForce(float angle, float power)
    {
        Vector3 vec = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle), 0);
        float p = Mathf.Lerp(20f, 1000f, power);
        rb.AddForce(vec * p);
    }
    
    
    /// <summary>
    /// Destroy the object
    /// </summary>
    [Server]
    private void DestroySelf()
    {
        NetworkServer.Destroy(gameObject);
    }


    /// <summary>
    /// When the projectile collides with another game object
    /// </summary>
    /// <param name="other"></param>
    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {
        // TODO explosion
        DestroySelf();
    }
    
    #endregion
}
