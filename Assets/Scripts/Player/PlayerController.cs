
using System;
using Mirror;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    [SerializeField]
    private float movementSpeed = 5f;
    [SerializeField]
    [Range(5, 20)]
    private float jumpForce = 5;
    

    [SerializeField]
    private CharacterController controller = null;
    private Vector2 previousInput;
    private float previousMovement;
    private bool isJumpTriggered;
    private float feetYCoordinates;
    [SerializeField]
    [Range(0.1f, 0.5f)]
    private float distToGround = 0.225f;
    


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
        if (gameObject.TryGetComponent(out Collider collider))
        {
            feetYCoordinates = collider.bounds.extents.y;
        }
        
        controls.Player.MoveQuadDir.performed += ctx => SetMovementQuadDir(ctx.ReadValue<Vector2>());
        controls.Player.MoveQuadDir.canceled += ctx => ResetMovementQuadDir();
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
    private void Update() => Move();

    
    [Client]
    private void SetMovement(float horizontal) => previousMovement = horizontal;
    [Client]
    private void ResetMovement() => previousMovement = 0f;
    
    [Client]
    private void SetMovementQuadDir(Vector2 movement) => previousInput = movement;
    [Client]
    private void ResetMovementQuadDir() => previousInput = Vector2.zero;

    private bool IsGrounded() => Physics.Raycast(transform.position, -Vector3.up, feetYCoordinates + distToGround);

    [Client]
    private void Move()
    {
        Vector3 movement = new Vector3(previousMovement, 0);
        if (isJumpTriggered == true)
            movement.y = jumpForce;
        // TODO else
 

        controller.Move(movement * (movementSpeed * Time.deltaTime));
    }
}
