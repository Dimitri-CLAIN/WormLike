using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerEntity : MonoBehaviour
{
    [Header("Turn Logic")]
    [SerializeField]
    private float playTime = 30f;
    [SerializeField]
    private PlayerTurn turnPrefab;
    private PlayerTurn playerInstance = null;

    [Header("Player Entity Fields")]
    [SerializeField]
    private Color ghostColor = new Color(1f, 0, 0, 0.2f);
    private Color originalColor;

    [SerializeField]
    private MeshRenderer meshRenderer;


    private void Start()
    {
        originalColor = Color.white;
        meshRenderer.material.color = originalColor;
    }

    [ContextMenu("Spawn Player Time")]
    public void SpawnPlayerTurn()
    {
        if (playerInstance != null) return;
        Debug.Log("<color=blue>" + "Spawn Player" + "</color>");
        playerInstance = Instantiate(turnPrefab, this.transform.position, this.transform.rotation);
        playerInstance.transform.SetParent(this.transform);
        playerInstance.SpawnPlayer(playTime);
        meshRenderer.material.color = ghostColor;
        playerInstance.OnPlayerTurnEnded += RestorePlayer;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            SpawnPlayerTurn();
    }


    private void RestorePlayer()
    {
        meshRenderer.material.color = originalColor;
        playerInstance.OnPlayerTurnEnded -= RestorePlayer;
    }
}
