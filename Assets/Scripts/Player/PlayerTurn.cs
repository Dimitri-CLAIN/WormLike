using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerTurn : MonoBehaviour
{
    private float timer;

    #region Movements

    [FormerlySerializedAs("velocity")]
    [SerializeField]
    [Range(1, 5)]
    private float speed = 3f;
    private Vector3 velocity;
    [SerializeField]
    [Range(5, 20)]
    private float jumpForce = 15f;

    [SerializeField]
    private Rigidbody rb;
    private float distToGround;

    

    #endregion

    private void Start()
    {
        if (gameObject.TryGetComponent<Collider>(out Collider collider))
        {
            distToGround = collider.bounds.extents.y;
        }
    }

    public void SpawnPlayer(float turnTimer)
    {
        StartCoroutine(InitTurnTimer(turnTimer));
    }


    private IEnumerator InitTurnTimer(float turnTimer)
    {
        // TODO a turn can also end if the player clicks a "Ready" || "End Turn" button
        timer = turnTimer;
        yield return new WaitForSeconds(timer);
        Destroy(this.gameObject); // sync to server so 
    }

    private void Update()
    {
        Move();
    }


    private void FixedUpdate()
    {
        rb.velocity = velocity;
    }


    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }
    
    
    private void Move()
    {
        // TODO disable movement inputs while in the air (no air control cuz its funny this wae)
        float horizontal = Input.GetAxis("Horizontal");

        velocity = new Vector3(horizontal * speed, 0, 0);
        if (Input.GetButton("Jump") && IsGrounded())
        {
            velocity.y = jumpForce;
        } else
        {
            velocity.y = rb.velocity.y;
        }
    }
    
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        // Vector3 direction = transform.TransformDirection(Vector3.down + 0.1f);
        Vector3 direction = new Vector3(0, -(distToGround + 0.1f), 0);
        Gizmos.DrawRay(transform.position, direction);
    }

#endif

}
