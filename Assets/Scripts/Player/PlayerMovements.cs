    
using System;
using Mirror;
using UnityEngine;

[RequireComponent(typeof(MyPlayer))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovements : NetworkBehaviour
{
    [SerializeField]
    private MyPlayer playerInstance;

    #region Movements
    [SerializeField]
    [Range(1, 5)]
    private float speed = 3f;
    private float previousInputHorizontal = 0f;
    private Vector3 velocity;
    private float feetYCoordinates;
    [SerializeField]
    [Range(0.1f, 0.5f)]
    private float distToGround = 0.1f;
    

    [SerializeField]
    [Range(5, 20)]
    private float jumpForce = 15f;
    [SerializeField]
    private Rigidbody rb;
    #endregion

    private void Start()
    {
        if (gameObject.TryGetComponent<Collider>(out Collider collider))
        {
            feetYCoordinates = collider.bounds.extents.y;
        }
    }


    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
        enabled = true;
    }


    [ClientCallback]
    private void Update()
    {
        velocity = rb.velocity;
        if (isOwned == false)
            return;
        foreach (var item in GameManager.instance.inputSettings)
        {
            if (item.Value.InputEnabled == true)
                Debug.Log("<color=green>" + item.Key.name + "INPUTS ENABLED" + "</color>");
            else
            {
                Debug.Log("<color=red>" + item.Key.name + "INPUTS DISABLED" + "</color>");
            }
        }
        if (GameManager.instance == null || 
            GameManager.instance.inputSettings.ContainsKey(playerInstance) == false || 
            GameManager.instance.inputSettings[playerInstance].InputEnabled == false)
            return; // not your turn to play

        CmdMove();
    }

    
    [ClientCallback]
    private void FixedUpdate()
    {
        rb.velocity = velocity;
    }
    
    
    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, feetYCoordinates + distToGround);
    }
    
    [Command]
    private void CmdMove()
    {
        // TODO disable movement inputs while in the air (no air control cuz its funny this wae)
        RpcMove();
    }


    [ClientRpc]
    private void RpcMove()
    {
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
        // Vector3 direction = transform.TransformDirection(Vector3.down + distToGround);
        Vector3 direction = new Vector3(0, -(feetYCoordinates + distToGround), 0);
        Gizmos.DrawRay(transform.position, direction);
    }

#endif

}
