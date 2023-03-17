using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer aimSprite;
    [SerializeField]
    private Worm worm;
    [SerializeField]
    private float angleSetter = 0f;
    private float aimAngle = 0f;
    [SerializeField]
    private Transform shoulder;
    
    [SerializeField]
    [Range(0.1f, 10f)]
    private float sensitivity = 1f;
    private float startChrono = 0f;
    private float chronoBuffer = 0f;
    [SerializeField]
    [Range(0.5f, 2f)]
    private float shotTime = 1f;

    #region BindControls
    
    private void BindHolsterWeapon(InputAction.CallbackContext ctx) => HolsterWeapon();
    private void BindDrawWeapon(InputAction.CallbackContext ctx) => DrawWeapon();
    private void BindSetAim(InputAction.CallbackContext ctx) => SetAim(ctx.ReadValue<float>());
    private void BindResetAim(InputAction.CallbackContext ctx) => ResetAim();
    private void BindStartShot(InputAction.CallbackContext ctx) => StartShot();
    private void BindReleaseShot(InputAction.CallbackContext ctx) => ReleaseShot();
    
    #endregion
    
    public void Start()
    {
        if (!worm.isOwned) return;
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

    private void OnDestroy()
    {
        if (!worm.isOwned) return;

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
        startChrono = chronoBuffer = Time.time;
        Debug.Log("<color=green>" + "START SHOOTING" + "</color>");
    }


    // [Client]
    // private void HoldShoot()
    // {
    //     // chronoBuffer += Time.deltaTime;
    //     // if (chronoBuffer >= startChrono + shotTime)
    //     //     worm.Controls.Player.Shoot.canceled
    // }


    [Client]
    private void ReleaseShot()
    {
        float shotPower = Time.time - startChrono;
        Debug.Log($"<color=red>RELEASE SHOT WITH VALUE {shotPower:F}</color>");
    }
    
    
    [ClientCallback]
    private void Update() => ApplyAim();


    [Client]
    private void ApplyAim()
    {
        if (angleSetter == 0f) return;

        // angle clamping between 0 and 360
        aimAngle %= 360f;
        if (aimAngle < 0f) aimAngle += 360f;
        
        transform.RotateAround(shoulder.position, Vector3.forward, angleSetter);
    }

    #endregion
    
    #region Server
    
    
    
    #endregion
}
