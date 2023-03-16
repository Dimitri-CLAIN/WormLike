using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Worm : NetworkBehaviour
{
    #region Visual
    public Color playColor = Color.red;
    private readonly Color originalColor = Color.white;
    [SyncVar(hook = nameof(HandleColorChanged))]
    private Color color;
    [SerializeField]
    private MeshRenderer meshRenderer;
    #endregion

    private GameManager gameManager;
    private GameManager GameManager
    {
        get
        {
            if (gameManager != null) return gameManager;
            return gameManager = GameManager.instance;
        }
    }
    
    #region Play
    [SerializeField]
    private PlayerController playerController;
    private bool isTurnActive = false;
    public bool IsTurnActive 
    { get => isTurnActive; private set => isTurnActive = value; }
    private Controls controls;
    public Controls Controls
    {
        get
        {
            if (controls != null) return controls;
            return controls = new Controls();
        }
    }
    
    public event Action OnTurnStarted;
    public event Action OnTurnEnded;

    #endregion

    
    #region Server

    /// <summary>
    /// Start the turn of the player client-wise
    /// </summary>
    [Server]
    public void StartTurn()
    {
        IsTurnActive = true;
        color = playColor;
        TargetToggleControls(this.connectionToClient, true);
        OnTurnStarted?.Invoke();
    }
    

    /// <summary>
    /// End turn of the player client-wise
    /// </summary>
    [Server]
    public void EndTurn()
    {
        IsTurnActive = false;
        color = originalColor;
        TargetToggleControls(this.connectionToClient, false);
        OnTurnEnded?.Invoke();
    }

    
    
    #endregion

    
    #region Client

    /// <summary>
    /// Everytime the color is changed on the server, it changes the color on all the clients
    /// </summary>
    /// <param name="oldColor">Old Color</param>
    /// <param name="newColor">New Color</param>
    [Client]
    private void HandleColorChanged(Color oldColor, Color newColor)
    {
        meshRenderer.material.color = newColor;
    }


    /// <summary>
    /// Enable or disable the controls of the targeted Client
    /// </summary>
    /// <param name="conn">Client targeted</param>
    /// <param name="toggle">Enable or disable Controls</param>
    [TargetRpc]
    private void TargetToggleControls(NetworkConnection conn, bool toggle)
    {
        if (toggle)
            Controls.Enable();
        else
            Controls.Disable();
    }
    
    #endregion
}