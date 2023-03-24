using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : NetworkBehaviour
{
    [SerializeField]
    private Worm worm;
    
    private Camera camera;
    public Camera Camera
    {
        get => camera;
        set
        {
            camera = value;
        }
    }
    
    private Vector3 startDragPosition;
    private bool isDragging;

    
    #region Bind Actions
    
    private void BindStartMoveCamera(InputAction.CallbackContext ctx) => StartCamera();
    private void BindMoveCamera(InputAction.CallbackContext ctx) => MoveCamera();
    private void BindStopCamera(InputAction.CallbackContext ctx) => StopCamera();
    private void BindZoomCamera(InputAction.CallbackContext ctx) => ZoomCamera(ctx.ReadValue<float>());
    
    #endregion
    

    public override void OnStartLocalPlayer()
    {
        camera = Camera.main;
        worm.Controls.Camera.Enable();
        worm.Controls.Camera.Move.started += BindStartMoveCamera;
        worm.Controls.Camera.Move.performed += BindMoveCamera;
        worm.Controls.Camera.Move.canceled += BindStopCamera;
        worm.Controls.Camera.Zoom.performed += BindZoomCamera;
    }


    public override void OnStopLocalPlayer()
    {
        worm.Controls.Camera.Move.started -= BindStartMoveCamera;
        worm.Controls.Camera.Move.performed -= BindMoveCamera;
        worm.Controls.Camera.Move.canceled -= BindStopCamera;
        worm.Controls.Camera.Zoom.performed -= BindZoomCamera;
    }


    #region Client


    [Client]
    private void StartCamera()
    {
        startDragPosition = GetMousePosition;
        isDragging = true;
        Debug.Log($"<color=green>start cam started</color>");
    }
    
    
    [Client]
    private void MoveCamera()
    {
        // TODO check maximum cam move value
        Debug.Log($"<color=yellow>move camera performed</color>");
        
    }


    // private IEnumerator DragUpdate()
    // {
    //     
    // }
    
    
    
    
    private Vector3 GetMousePosition => camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());


    [Client]
    private void StopCamera()
    {
        Debug.Log($"<color=red>stop camera move</color>");
        isDragging = false;
    }


    [Client]
    private void ZoomCamera(float value)
    {
        // TODO
        Debug.Log($"<color=blue>zoom camera {value}</color>");
    }

    Vector3 difference;
    [Client]
    private void LateUpdate()
    {
        if (isDragging == false) return;
        
        difference = GetMousePosition - camera.transform.position;
        camera.transform.position = startDragPosition - difference;
    }

    #endregion
}
