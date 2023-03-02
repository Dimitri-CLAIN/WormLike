using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInputSettings
{
    public bool InputEnabled { get; set; } = false;
}

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private float turnTime = 10f;
    [SerializeField]
    private float intervalTime = 2f;
    [SerializeField]
    private List<MyPlayer> players = new List<MyPlayer>();
    

    public static GameManager instance;
    public readonly Dictionary<MyPlayer, UserInputSettings> inputSettings = new Dictionary<MyPlayer, UserInputSettings>();

    private void Awake()
    {
        instance = this;
        foreach (MyPlayer player in players)
        {
            inputSettings[player] = new UserInputSettings();
        }
    }


    [ContextMenu("Launch Game")]
    private void LaunchGame()
    {
        StartCoroutine(HandleTurn());
    }


    [ContextMenu("Check Variables")]
    private void CheckVariables()
    {
        foreach (MyPlayer p in players)
        {
            Debug.Log("<color=red>" + inputSettings[p] + "</color>");
        }

        Debug.Log("<color=blue>" + players[0] == players[1] + "</color>");
        Debug.Log("<color=blue>" + inputSettings[players[0]] == inputSettings[players[1]] + "</color>");
    }
    
    
    private IEnumerator HandleTurn()
    {
        foreach (MyPlayer player in players)
        {
            Debug.Log("<color=yellow>" + "turn => " + player.name + "</color>");
            inputSettings[player].InputEnabled = true;
            player.PlayTurn(turnTime);
            yield return new WaitUntil(() => player.HasTurnEnded == true);
            inputSettings[player].InputEnabled = false;
            yield return new WaitForSeconds(intervalTime);
        }
    }
}
