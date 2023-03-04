using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MyPlayer : MonoBehaviour
{
    public Color playColor = Color.red;
    private Color originalColor = Color.white;
    [SerializeField]
    private MeshRenderer meshRenderer;
    
    public event Action OnTurnEnded;
    
    
    [ContextMenu("Play Turn")]
    public void PlayTurn(float turnTime)
    {
        // hasTurnEnded = false;
        meshRenderer.material.color = playColor;
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
        meshRenderer.material.color = originalColor;
        OnTurnEnded?.Invoke();
    }
}
