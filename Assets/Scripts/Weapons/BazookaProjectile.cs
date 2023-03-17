using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class BazookaProjectile : NetworkBehaviour
{
    public Rigidbody rb;

    [Server]
    private void DestroySelf()
    {
        NetworkServer.Destroy(gameObject);
    }


    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {
        // TODO explosion
        DestroySelf();
    }
}
