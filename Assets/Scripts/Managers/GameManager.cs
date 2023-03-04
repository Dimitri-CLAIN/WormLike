using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    private bool hasTurnEnded = false;

    private void Awake()
    {
        instance = this;
        foreach (MyPlayer player in players)
        {

            inputSettings[player] = new UserInputSettings();
            player.OnTurnEnded += UpdateEndTurn;
        }
    }


    [ContextMenu("Launch Game")]
    private void LaunchGame()
    {
        StartCoroutine(HandleTurn());
    }


    private bool ConditionForEndOfGameIsTrue()
    {
        // TODO
        return false;
    }
    
    private IEnumerator HandleTurn()
    {
        while (true)
        {
            if (ConditionForEndOfGameIsTrue())
                yield break;
            foreach (MyPlayer player in players)
            {
                inputSettings[player].InputEnabled = true;
                player.PlayTurn(turnTime);
                yield return new WaitUntil(() => hasTurnEnded == true);
                inputSettings[player].InputEnabled = false;
                hasTurnEnded = false;
                yield return new WaitForSeconds(intervalTime);
            }
        }
    }


    private void UpdateEndTurn()
    {
        hasTurnEnded = true;
    }


    public void AddPlayer(MyPlayer p)
    {
        if (false) // max nb players or party already running
            Debug.Log("<color=red>" + "Cannot add a player" + "</color>");
        players.Add(p);
        inputSettings[p] = new UserInputSettings();
        p.OnTurnEnded += UpdateEndTurn;
    }
}
