using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class MapHealth : Health
{
    [SerializeField]
    private Renderer blockRenderer;
    [SerializeField]
    private List<Material> materialsStade;

    private void Start() => blockRenderer = gameObject.GetComponent<Renderer>();
    
    public override void OnStartServer()
    {
        OnDeath += DestroyBlock;
        blockRenderer = GetComponent<Renderer>();
        base.OnStartServer();
    }

    public override void OnStartClient()
    {
        blockRenderer = GetComponent<Renderer>();
    }


    #region Server

    /// <summary>
    /// Add value of health to the map component
    /// </summary>
    /// <param name="value">Amount of health</param>
    [ServerCallback]
    public override void Add(int value)
    {
        base.Add(value);
    }


    /// <summary>
    /// Subtract value of health to the map component
    /// </summary>
    /// <param name="value">Ignored value, it will always subtract only 1 HP</param>
    [ServerCallback]
    public override void Remove(int value)
    {
        base.Remove(1);
    }

    
    /// <summary>
    /// Destroy the block server wise
    /// </summary>
    [Server]
    private void DestroyBlock()
    {
        NetworkServer.Destroy(this.gameObject);
    }

    #endregion


    #region Client

    /// <summary>
    /// Attribute a color on the current map component depending its health
    /// </summary>
    [Client]
    private void AttributeColor()
    {
        if (health is <= 0 or > 3) return;

        if (blockRenderer == null)
        {
            Debug.Log($"<color=red>No Renderer found on this map component</color>");
            return;
        }
        blockRenderer.material = materialsStade[health -1];
    }
    

    /// <summary>
    /// Sync method called on all the clients when the health is changed on the server
    /// </summary>
    /// <param name="oldHealth">Old Health value</param>
    /// <param name="newHealth">New Health value</param>
    [Client]
    protected override void HandleHealthChange(int oldHealth, int newHealth)
    {
        base.HandleHealthChange(oldHealth, newHealth);
        AttributeColor();
    }

    #endregion
}
