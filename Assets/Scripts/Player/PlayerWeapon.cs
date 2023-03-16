using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer aimSprite;
    [SerializeField]
    private Worm worm;
    [FormerlySerializedAs("previousAngle")]
    [SerializeField]
    private float aimAngle = 0f;
    [SerializeField]
    private Transform shoulder;
    
    [SerializeField]
    [Range(0.1f, 10f)]
    private float sensitivity = 1f;
    


    public void Start()
    {
        if (!worm.isOwned) return;
        enabled = true;
        
        worm.Controls.Player.Move.performed += ctx => HolsterWeapon();
        worm.Controls.Player.Jump.performed += ctx => HolsterWeapon();
        worm.Controls.Player.Jump.canceled += ctx => DrawWeapon();
        worm.Controls.Player.Move.canceled += ctx => DrawWeapon();
        worm.Controls.Player.Aim.performed += ctx => SetAim(ctx.ReadValue<float>());
        worm.Controls.Player.Aim.canceled += ctx => ResetAim();
        worm.OnTurnStarted += DrawWeapon;
        worm.OnTurnEnded += HolsterWeapon;
    }

    private void OnDestroy()
    {
        worm.OnTurnStarted -= DrawWeapon;
        worm.OnTurnEnded -= HolsterWeapon;
    }


    #region Client

    [Client]
    private void SetAim(float value) => aimAngle = value * sensitivity;
    
    
    [Client]
    private void ResetAim() => aimAngle = 0f;



    [Client]
    private void HolsterWeapon()
    {
        aimSprite.enabled = false;
        ResetAim();
    }

    [Client]
    private void DrawWeapon() => aimSprite.enabled = true;
    
    
    [ClientCallback]
    private void Update() => ApplyAim();


    [Client]
    private void ApplyAim()
    {
        if (aimAngle == 0f) return;
        transform.RotateAround(shoulder.position, Vector3.forward, aimAngle);
    }

    #endregion
    
    #region Server
    
    
    
    #endregion
}
