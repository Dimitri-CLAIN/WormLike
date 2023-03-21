
using System;
using Mirror;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : NetworkBehaviour
{
    #region Game Objects

    [Header("Game Objects references")]
    [SerializeField]
    private CharacterController controller = null;
    [SerializeField]
    private Worm worm;

    #endregion

    #region Movement Values

    [Header("Movement Values")]
    [SerializeField]
    private float movementSpeed = 5f;
    [SerializeField]
    [Range(1,10)]
    private float jumpForce = 5;
    Vector3 direction = Vector3.zero;
    private float gravity = -9.81f;
    [SerializeField]
    [Range(1,10)]
    private float gravityMultiplier = 5f;
    private float velocity;
    private float previousMovement;
    private bool isJumpTriggered;

    #endregion


    public override void OnStartAuthority()
    {
        enabled = true;
        
        worm.Controls.Player.Move.performed += ctx => SetMovement(ctx.ReadValue<float>());
        worm.Controls.Player.Move.canceled += ctx => ResetMovement();
        worm.Controls.Player.Jump.performed += ctx => isJumpTriggered = true; // only true if turn is active
    }

    public override void OnStartClient()
    {
        worm.Controls.Player.Disable();
    }


    [ClientCallback]
    private void Update()
    {
        direction = new Vector3(previousMovement, 0);
        ApplyGravity();
        ApplyJump();
        ApplyMovement();
    }

    
    [Client]
    private void SetMovement(float horizontal)
    {
        previousMovement = horizontal;
    }

    [Client]
    private void ResetMovement() => previousMovement = 0f;
    
    [Client]
    private void ApplyGravity()
    {
        if (IsGrounded() && velocity < 0.0f)
        {
            velocity = -1.0f;
        } else
        {
            velocity += gravity * gravityMultiplier * Time.deltaTime;
        }
        direction.y = velocity;
    }

    [Client]
    private void ApplyMovement() => controller.Move(direction * (movementSpeed * Time.deltaTime));
    
    
    [Client]
    private void ApplyJump()
    {
        if (isJumpTriggered == true)
        {
            if (controller.isGrounded == true)
            {
                velocity += jumpForce;
            }
            isJumpTriggered = false;
        }
    }

    [Client]
    public bool IsGrounded() => controller.isGrounded;
}
