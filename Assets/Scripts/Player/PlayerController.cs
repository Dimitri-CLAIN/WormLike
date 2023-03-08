
using System;
using Mirror;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
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


    [SerializeField]
    private CharacterController controller = null;
    private Vector2 previousInput;
    private float previousMovement;
    private bool isJumpTriggered;
    private float feetYCoordinates;


    private Controls controls;
    public Controls Controls
    {
        get
        {
            if (controls != null) return controls;
            return controls = new Controls();
        }
    }

    public override void OnStartAuthority()
    {
        enabled = true;
        
        controls.Player.Move.performed += ctx => SetMovement(ctx.ReadValue<float>());
        controls.Player.Move.canceled += ctx => ResetMovement();
        controls.Player.Jump.performed += ctx => isJumpTriggered = true;
    }


    [ClientCallback]
    private void OnEnable()
    {
        Controls.Enable();
    }
    [ClientCallback]
    private void OnDisable()
    {
        Controls.Disable();
    }

    [ClientCallback]
    private void Update()
    {
        direction = new Vector3(previousMovement, 0);
        // velocity.y = rb.velocity.y;
        ApplyGravity();
        Move();
    }

    
    [Client]
    private void SetMovement(float horizontal) => previousMovement = horizontal;
    [Client]
    private void ResetMovement() => previousMovement = 0f;
    
    [Client]
    private void ApplyGravity()
    {
        if (controller.isGrounded && velocity < 0.0f)
        {
            velocity = -1.0f;
        } else
        {
            velocity += gravity * gravityMultiplier * Time.deltaTime;
        }
        direction.y = velocity;
    }
    
    
    [Client]
    private void Move()
    {
        if (isJumpTriggered == true)
        {
            if (controller.isGrounded == true)
            {
                velocity += jumpForce;
            }
            isJumpTriggered = false;
        }

        controller.Move(direction * (movementSpeed * Time.deltaTime));
    }
}
