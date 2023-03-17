using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerWeapon : NetworkBehaviour
{
    #region Gameobjects
    
    [SerializeField]
    private SpriteRenderer aimSprite;
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
        
        worm.Controls.Player.Move.performed += BindHolsterWeapon;
        worm.Controls.Player.Jump.performed += BindHolsterWeapon;
        worm.Controls.Player.Jump.canceled += BindDrawWeapon;
        worm.Controls.Player.Move.canceled += BindDrawWeapon;
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
        worm.Controls.Player.Move.performed -= BindHolsterWeapon;
        worm.Controls.Player.Jump.performed -= BindHolsterWeapon;
        worm.Controls.Player.Jump.canceled -= BindDrawWeapon;
        worm.Controls.Player.Move.canceled -= BindDrawWeapon;
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
    /// <param name="value">from -0 to 1</param>
    [Client]
    private void SetAim(float value) => angleSetter = value * sensitivity;
    
    
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
        aimSprite.enabled = false;
        ResetAim();
    }


    /// <summary>
    /// Display the crosshair, player's ready to shoot
    /// </summary>
    [Client]
    private void DrawWeapon() => aimSprite.enabled = true;


    /// <summary>
    /// Start the shot
    /// </summary>
    [Client]
    private void StartShot()
    {
        startChrono = Time.time;
    }


    /// <summary>
    /// Release the shot
    /// </summary>
    [Client]
    private void ReleaseShot()
    {
        float shotPower = Time.time - startChrono;
        // TODO weapon selection
        CmdFireBazooka(shotPower);
    }
    

    [ClientCallback]
    private void Update() => ApplyAim();


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
    /// Create the bazooka projectile and send the isntruction to the server
    /// </summary>
    /// <param name="shotPower">Normalized shot power</param>
    [Command]
    private void CmdFireBazooka(float shotPower)
    {
        projectileBazookaInstance = Instantiate(bazookaProjectilePrefab, crosshairTransform.position, crosshairTransform.rotation);
        NetworkServer.Spawn(projectileBazookaInstance.gameObject);
        projectileBazookaInstance.RpcAddForce(aimAngle, shotPower);
    }
    
    #endregion
}
