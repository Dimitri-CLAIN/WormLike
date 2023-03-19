using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class DestroyParticleOnStop : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem particle;

    private void Awake()
    {
        if (particle == null) particle = GetComponent<ParticleSystem>();
    }

    private void OnParticleSystemStopped()
    {
        NetworkServer.Destroy(this.gameObject);
        Destroy(this);
    }


    private void Update()
    {
        if (particle.IsAlive() == false)
        {
            NetworkServer.Destroy(this.gameObject);
            Destroy(this);
        }
    }
}
