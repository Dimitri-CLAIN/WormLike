
using System;
using KawaiiImplementation;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : NetworkBehaviour
{
    #region Game Objects

    [Header("Game Objects references")]
    [SerializeField]
    private CharacterController controller = null;
    [SerializeField]
    private Worm worm;

    // Slimes
    [SerializeField]
    private Transform slimeHolder;
    public AnimateSlime slimeAnimator;
    

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
    
    //slime
    [SerializeField]
    [Range(0, 90)]
    private float lookDirectionAngleDiff = 35f;
    

    #endregion

    #region Bind Actions

    private void BindSetMovement(InputAction.CallbackContext ctx) => SetMovement(ctx.ReadValue<float>());
    private void BindResetMovement(InputAction.CallbackContext ctx) => ResetMovement();
    private void BindTriggerJump(InputAction.CallbackContext ctx) => TriggerJump();
    
    // Slime Actions
    private void BindSlimeMovement(InputAction.CallbackContext ctx) => SetSlimeMovement(ctx.ReadValue<float>());
    private void BindSlimeIdle(InputAction.CallbackContext ctx) => SetSlimeIdle();
    private void BindSlimeJump(InputAction.CallbackContext ctx) => TriggerSlimeJump();

    #endregion
    

    public override void OnStartAuthority()
    {
        enabled = true;
        
        worm.Controls.Player.Move.performed += BindSetMovement;
        worm.Controls.Player.Move.canceled += BindResetMovement;
        worm.Controls.Player.Jump.performed += BindTriggerJump;
        
        // slime
        worm.Controls.Player.Move.performed += BindSlimeMovement;
        worm.Controls.Player.Move.canceled += BindSlimeIdle;
        worm.Controls.Player.Jump.performed += BindSlimeJump;
    }

    public override void OnStopAuthority()
    {
        worm.Controls.Player.Move.performed -= BindSetMovement;
        worm.Controls.Player.Move.canceled -= BindResetMovement;
        worm.Controls.Player.Jump.performed -= BindTriggerJump;
        
        // slime
        worm.Controls.Player.Move.performed -= BindSlimeMovement;
        worm.Controls.Player.Move.canceled -= BindSlimeIdle;
        worm.Controls.Player.Jump.performed -= BindSlimeJump;
    }

    public override void OnStartClient()
    {
        worm.Controls.Player.Disable();
    }

    #region Client

    [ClientCallback]
    private void Update()
    {
        direction = new Vector3(previousMovement, 0);
        ApplyGravity();
        ApplyJump();
        ApplyMovement();
    }

    
    /// <summary>
    /// Sets the movement value
    /// </summary>
    /// <param name="horizontal">-1 is left, 1 is right</param>
    [Client]
    private void SetMovement(float horizontal)
    {
        previousMovement = horizontal;
    }

    
    /// <summary>
    /// Resets the value of horizontal movement so the player idles
    /// </summary>
    [Client]
    private void ResetMovement() => previousMovement = 0f;
    
    
    /// <summary>
    /// Triggers the player's jump
    /// </summary>
    [Client]
    private void TriggerJump() => isJumpTriggered = true;

    
    /// <summary>
    /// Apply gravity to the Player Controller
    /// </summary>
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

    
    /// <summary>
    /// Apply the movement depending on what has been registered with the inputs
    /// </summary>
    [Client]
    private void ApplyMovement() => controller.Move(direction * (movementSpeed * Time.deltaTime));
    
    
    /// <summary>
    /// Apply the jump if it has been triggered and if the player is grounded
    /// </summary>
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

    
    /// <summary>
    /// Checks if the Player Controller is grounded
    /// </summary>
    /// <returns></returns>
    [Client]
    public bool IsGrounded() => controller.isGrounded;

    #endregion

    #region Slime

    /// <summary>
    /// Sets the slime movement animation and which way it faces
    /// <para>Not used as the animation moves on the Z axis</para>
    /// </summary>
    /// <param name="value">-1 is left, 1 is right</param>
    [Client]
    private void SetSlimeMovement(float value)
    {
        float signMultiplier = value > 0 ? 1 : -1;
        slimeHolder.localEulerAngles = new Vector3(0, 180 - (signMultiplier * lookDirectionAngleDiff), 0);
        
        if (slimeAnimator != null)
            slimeAnimator.currentState = KawaiiImplementation.SlimeAnimationState.Walk;
    }


    /// <summary>
    /// Set the slime anim to idle
    /// </summary>
    [Client]
    private void SetSlimeIdle()
    {
        if (slimeAnimator != null)
            slimeAnimator.currentState = KawaiiImplementation.SlimeAnimationState.Idle;
    }


    /// <summary>
    /// Triggers the slime's jump animation
    /// </summary>
    [Client]
    private void TriggerSlimeJump()
    {
        if (slimeAnimator == null)
            return;
        slimeAnimator.currentState = IsGrounded() ? KawaiiImplementation.SlimeAnimationState.Idle : KawaiiImplementation.SlimeAnimationState.Jump;
    }

    #endregion
}
