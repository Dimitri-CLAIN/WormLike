using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

/// <summary>
/// Handles the camera control for the local player
/// </summary>
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
    
    #region Drag Camera
    
    private Vector3 dragStart;
    private Vector3 difference;
    private bool isDragging;
    [SerializeField]
    private float groundZ;
    
    #endregion
    
    #region Zoom Camera

    [SerializeField]
    private float zoomValue = 1f;
    [SerializeField]
    private Vector2 zoomLimits = new Vector2(-2, -25);
    
    #endregion
    

    
    #region Bind Actions
    
    private void BindStartMoveCamera(InputAction.CallbackContext ctx) => StartCamera();
    private void BindStopCamera(InputAction.CallbackContext ctx) => StopCamera();
    private void BindZoomCamera(InputAction.CallbackContext ctx) => ZoomCamera(ctx.ReadValue<float>());
    private void BindResetCamera(InputAction.CallbackContext ctx) => ResetCamera();
    
    #endregion
    

    public override void OnStartLocalPlayer()
    {
        camera = Camera.main;
        camera.transform.SetParent(worm.transform);
        worm.Controls.Camera.Enable();
        worm.Controls.Camera.Move.started += BindStartMoveCamera;
        worm.Controls.Camera.Move.canceled += BindStopCamera;
        worm.Controls.Camera.Zoom.performed += BindZoomCamera;
        worm.Controls.Camera.Reset.performed += BindResetCamera;

        worm.Controls.Player.Move.performed += BindResetCamera;
    }


    public override void OnStopLocalPlayer()
    {
        worm.Controls.Camera.Disable();
        worm.Controls.Camera.Move.started -= BindStartMoveCamera;
        worm.Controls.Camera.Move.canceled -= BindStopCamera;
        worm.Controls.Camera.Zoom.performed -= BindZoomCamera;
        worm.Controls.Camera.Reset.performed -= BindResetCamera;
        
        worm.Controls.Player.Move.performed -= BindResetCamera;
    }


    #region Client


    /// <summary>
    /// Once the player clicks, it registers the click position and starts the drag action
    /// </summary>
    [Client]
    private void StartCamera()
    {
        dragStart = GetMouseWorldPosition(groundZ);
        isDragging = true;
    }


    /// <summary>
    /// Stops the drag action
    /// </summary>
    [Client]
    private void StopCamera() => isDragging = false;
    
    
    /// <summary>
    /// Reset the camera position to focus the player
    /// </summary>
    [Client]
    private void ResetCamera() => camera.transform.position = new Vector3(worm.transform.position.x, worm.transform.position.y, camera.transform.position.z);


    /// <summary>
    /// Method used to get the mouse position on the world, with the groundZ as the depth of contact
    /// </summary>
    /// <param name="z">Z level of contact for the raycast</param>
    /// <returns>Position of the mouse in the world</returns>
    private Vector3 GetMouseWorldPosition(float z)
    {
        Ray mousePos = camera.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.forward, new Vector3(0,0,z));
        float distance;
        ground.Raycast(mousePos, out distance);
        return mousePos.GetPoint(distance);
    }


    /// <summary>
    /// Zoom in or out with Mouse Wheel to the defined boundaries
    /// </summary>
    /// <param name="value">Positive for zoom in, Negative for zoom out</param>
    [Client]
    private void ZoomCamera(float value)
    {
        float newZoomValue = 0;
        if (value > 0) // zoom in
        {
            newZoomValue = Mathf.Min(zoomLimits.x, camera.transform.position.z + zoomValue);
        } else
        {
            newZoomValue = Mathf.Max(zoomLimits.y, camera.transform.position.z - zoomValue);
        }
        camera.transform.position = new Vector3(camera.transform.position.x, camera.transform.position.y, newZoomValue);
    }

    
    [Client]
    private void LateUpdate()
    {
        if (isDragging == false) return;
        
        Vector3 diff = dragStart - GetMouseWorldPosition(groundZ);
        camera.transform.position += diff;

        if (Input.GetKeyDown(KeyCode.P))
        {
            camera.transform.position = new Vector3(camera.transform.position.x + 2, camera.transform.position.y, camera.transform.position.z);
        }
    }

    #endregion
}
