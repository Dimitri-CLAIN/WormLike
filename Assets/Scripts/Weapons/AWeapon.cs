
using Mirror;
using UnityEngine;

public abstract class AWeapon : NetworkBehaviour
{
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private ParticleSystem particles;
    

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
    }

    
    #endregion
}
