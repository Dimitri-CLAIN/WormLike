using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerWeapon : NetworkBehaviour
{
    #region Gameobjects
    
    [SerializeField]
    private Image aimImage;
    [SerializeField]
    private Image powerIndicator;
    
    
    [SerializeField]
    private Transform shoulder;
    [SerializeField]
    private Transform crosshairTransform;
    [SerializeField]
    private Worm worm;
    private BazookaProjectile projectileBazookaInstance = null;
    #endregion

    #region ShotAngle

    [SerializeField]
    private float angleSetter = 0f;
    private float aimAngle = 0f;
    [SerializeField]
    [Range(0.1f, 10f)]
    private float sensitivity = 1f;
    
    #endregion

    private bool isShotTriggered = false;
    private float startChrono = 0f;

    [SerializeField]
    private BazookaProjectile bazookaProjectilePrefab;
    
    
    #region BindControls
    
    private void BindHolsterWeapon(InputAction.CallbackContext ctx) => HolsterWeapon();
    private void BindDrawWeapon(InputAction.CallbackContext ctx) => DrawWeapon();
    private void BindSetAim(InputAction.CallbackContext ctx) => SetAim(ctx.ReadValue<float>());
    private void BindResetAim(InputAction.CallbackContext ctx) => ResetAim();
    private void BindStartShot(InputAction.CallbackContext ctx) => StartShot();
    private void BindReleaseShot(InputAction.CallbackContext ctx) => ReleaseShot();
    
    #endregion

    public override void OnStartAuthority()
    {
        enabled = true;
        
        worm.Controls.Player.Jump.performed += BindHolsterWeapon;
        worm.Controls.Player.Jump.canceled += BindDrawWeapon;
        worm.Controls.Player.Aim.performed += BindSetAim;
        worm.Controls.Player.Aim.canceled += BindResetAim;
        worm.Controls.Player.Shoot.started += BindStartShot;
        worm.Controls.Player.Shoot.performed += BindReleaseShot;
        worm.Controls.Player.Shoot.canceled += BindReleaseShot;
        worm.OnTurnStarted += DrawWeapon;
        worm.OnTurnEnded += HolsterWeapon;
    }

    public override void OnStopAuthority()
    {
        worm.Controls.Player.Jump.performed -= BindHolsterWeapon;
        worm.Controls.Player.Jump.canceled -= BindDrawWeapon;
        worm.Controls.Player.Aim.performed -= BindSetAim;
        worm.Controls.Player.Aim.canceled -= BindResetAim;
        worm.Controls.Player.Shoot.started -= BindStartShot;
        worm.Controls.Player.Shoot.performed -= BindReleaseShot;
        worm.Controls.Player.Shoot.canceled -= BindReleaseShot;

        worm.OnTurnStarted -= DrawWeapon;
        worm.OnTurnEnded -= HolsterWeapon;
    }


    #region Client

    /// <summary>
    /// Set the angle setter to change the angle depending on the value from the Input Action
    /// </summary>
    /// <param name="value">from -1 to 1</param>
    [Client]
    private void SetAim(float value)
    {
        if (worm.Controller.IsGrounded() == false) return;
        angleSetter = value * sensitivity;
    }
    
    
    /// <summary>
    /// Set the angle setter to 0 to stop the angle's update
    /// </summary>
    [Client]
    private void ResetAim() => angleSetter = 0f;


    /// <summary>
    /// Hide the crosshair as the player is moving
    /// </summary>
    [Client]
    private void HolsterWeapon()
    {
        aimImage.enabled = powerIndicator.enabled = false;
        ResetAim();
    }


    /// <summary>
    /// Display the crosshair, player's ready to shoot
    /// </summary>
    [Client]
    private void DrawWeapon() => aimImage.enabled = true;


    /// <summary>
    /// Start the shot
    /// </summary>
    [Client]
    private void StartShot()
    {
        startChrono = Time.time;
        isShotTriggered = true;
        powerIndicator.enabled = true;
        worm.Controls.Player.Move.Disable();
    }


    /// <summary>
    /// Release the shot
    /// </summary>
    [Client]
    private void ReleaseShot()
    {
        float shotPower = Time.time - startChrono;
        CmdFireBazooka(aimAngle, shotPower);
        isShotTriggered = false;
        worm.Controls.Player.Move.Enable();
    }


    /// <summary>
    /// Update the power indicator 
    /// </summary>
    [ClientCallback]
    private void UpdatePowerIndicator() => powerIndicator.fillAmount = Time.time - startChrono;
    

    [ClientCallback]
    private void Update()
    {
        ApplyAim();
        if (isShotTriggered)
            UpdatePowerIndicator();
    }


    /// <summary>
    /// Update the angle if the angle setter has been changed in the frame
    /// </summary>
    [Client]
    private void ApplyAim()
    {
        if (angleSetter == 0f) return;

        aimAngle += angleSetter;
        // angle clamping between 0 and 360
        aimAngle %= 360f;
        if (aimAngle < 0f) aimAngle += 360f;
        
        crosshairTransform.RotateAround(shoulder.position, Vector3.forward, angleSetter);
    }
    
    #endregion  
    
    #region Server

    /// <summary>
    /// Create the bazooka projectile and send the instruction to the server
    /// </summary>
    /// <param name="angle">Angle of the shot</param>
    /// <param name="shotPower">Normalized shot power</param>
    [Command]
    private void CmdFireBazooka(float angle, float shotPower)
    {
        projectileBazookaInstance = Instantiate(bazookaProjectilePrefab, crosshairTransform.position, crosshairTransform.rotation);
        NetworkServer.Spawn(projectileBazookaInstance.gameObject);
        projectileBazookaInstance.RpcFireWeapon(angle, shotPower);
    }
    
    #endregion
}
