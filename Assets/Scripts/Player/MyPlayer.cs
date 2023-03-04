using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using Unity.VisualScripting;
using UnityEngine;

public class MyPlayer : NetworkBehaviour
{
    public Color playColor = Color.red;
    private Color originalColor = Color.white;
    [SyncVar(hook = nameof(HandleColorChanged))]
    private Color color;
    [SerializeField]
    private MeshRenderer meshRenderer;
    
    public event Action OnTurnEnded;
    
    
    [ContextMenu("Play Turn")]
    public void PlayTurn(float turnTime)
    {
        // hasTurnEnded = false;
        color = playColor;
        StartCoroutine(StartTurn(turnTime));
    }


    private IEnumerator StartTurn(float turnTime)
    {
        yield return new WaitForSeconds(turnTime);
        EndTurn();
    }


    public void EndTurn()
    {
        // restore color
        color = originalColor;
        OnTurnEnded?.Invoke();
    }


    [Client]
    private void HandleColorChanged(Color oldColor, Color newColor)
    {
        meshRenderer.material.color = newColor;
    }
}
