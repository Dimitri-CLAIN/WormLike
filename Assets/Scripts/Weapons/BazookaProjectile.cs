using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class BazookaProjectile : AWeapon
{

    /// <summary>
    /// When the projectile collides with another game object
    /// </summary>
    /// <param name="other"></param>
    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {
        DestroySelf();
    }


}
