using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity : MonoBehaviour
{
    [SerializeField]
    private float playTime = 30f;
    [SerializeField]
    private PlayerTurn turnPrefab;
    private PlayerTurn playerInstance = null;

    [ContextMenu("Spawn Player Time")]
    public void SpawnPlayerTurn()
    {
        Debug.Log("<color=blue>" + "Spawn Player" + "</color>");
        playerInstance = Instantiate(turnPrefab, this.transform.position, this.transform.rotation);
        playerInstance.transform.SetParent(this.transform);
        playerInstance.SpawnPlayer(playTime);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            SpawnPlayerTurn();
    }
}
