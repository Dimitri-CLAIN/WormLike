
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
    public bool isTurnActive { get; set; } = false;


    [SerializeField]
    private CharacterController controller = null;
    [SerializeField]
    private Worm player;
    

    private Vector2 previousInput;
    private float previousMovement;
    private bool isJumpTriggered;
    private float feetYCoordinates;

    private GameManager gameManager;

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
        
        Controls.Player.Move.performed += ctx => SetMovement(ctx.ReadValue<float>());
        Controls.Player.Move.canceled += ctx => ResetMovement();
        Controls.Player.Jump.performed += ctx => isJumpTriggered = true; // only true if turn is active
    }

    public override void OnStartClient()
    {
        Controls.Player.Disable();
    }


    // [ClientCallback]
    // private void OnEnable()
    // {
    //     Controls.Enable();
    // }
    // [ClientCallback]
    // private void OnDisable()
    // {
    //     Controls.Disable();
    // }

    [ClientCallback]
    private void Update()
    {
        direction = new Vector3(previousMovement, 0);
        ApplyGravity();
        Move();
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
    private void ApplyMovement() => controller.Move(direction * (movementSpeed * Time.deltaTime));
    
    
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
    }
}
