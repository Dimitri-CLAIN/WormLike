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

    [Client]
    private void SetAim(float value) => angleSetter = value * sensitivity;
    
    
    [Client]
    private void ResetAim() => angleSetter = 0f;



    [Client]
    private void HolsterWeapon()
    {
        aimSprite.enabled = false;
        ResetAim();
    }

    [Client]
    private void DrawWeapon() => aimSprite.enabled = true;


    [Client]
    private void StartShot()
    {
        startChrono = Time.time;
    }


    [Client]
    private void ReleaseShot()
    {
        float shotPower = Time.time - startChrono;
        // TODO weapon selection
        CmdFireBazooka(shotPower);
    }
    
    
    [ClientCallback]
    private void Update() => ApplyAim();


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


    [ClientRpc]
    private void RpcFireBazooka(float shotPower)
    {
        if (projectileBazookaInstance == null) return;

        Vector3 vec = new Vector3(Mathf.Cos(Mathf.Deg2Rad * aimAngle), Mathf.Sin(Mathf.Deg2Rad * aimAngle), 0);
        float power = Mathf.Lerp(20f, 1000f, shotPower);
        projectileBazookaInstance.rb.AddForce(vec * power);
    }
    
    #endregion
    
    #region Server

    [Command]
    private void CmdFireBazooka(float shotPower)
    {
        projectileBazookaInstance = Instantiate(bazookaProjectilePrefab, crosshairTransform.position, crosshairTransform.rotation);
        NetworkServer.Spawn(projectileBazookaInstance.gameObject);
        RpcFireBazooka(shotPower);
    }
    
    #endregion
}
