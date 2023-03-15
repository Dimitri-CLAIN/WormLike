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
    #endregion

    
    #region Server

    /// <summary>
    /// Start the turn of the player client-wise
    /// </summary>
    [Server]
    public void StartTurn()
    {
        color = playColor;
        TargetToggleControls(this.connectionToClient, true);        
    }
    

    /// <summary>
    /// End turn of the player client-wise
    /// </summary>
    [Server]
    public void EndTurn()
    {
        color = originalColor;
        TargetToggleControls(this.connectionToClient, false);        
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
            playerController.Controls.Enable();
        else
            playerController.Controls.Disable();
    }
    
    #endregion
}