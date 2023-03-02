using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MyPlayer : MonoBehaviour
{
    [SerializeField]
    private Color playColor = Color.red;
    private Color originalColor = Color.white;
    [SerializeField]
    private MeshRenderer meshRenderer;
    private bool hasTurnEnded = false;

    public bool HasTurnEnded
    {
        get => hasTurnEnded;
    }
    

    [ContextMenu("Play Turn")]
    public void PlayTurn(float turnTime)
    {
        hasTurnEnded = false;
        meshRenderer.material.color = playColor;
        StartCoroutine(StartTurn(turnTime));
    }


    private IEnumerator StartTurn(float turnTime)
    {
        yield return new WaitForSeconds(turnTime);
        EndTurn();
    }


    private void EndTurn()
    {
        // restore color
        meshRenderer.material.color = originalColor;
        hasTurnEnded = true;
    }
}
