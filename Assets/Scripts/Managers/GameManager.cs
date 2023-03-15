using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Random = System.Random;

public class UserInputSettings
{
    public bool InputEnabled { get; set; } = false;
    private Controls controls;
    public Controls Controls
    {
        get
        {
            if (controls != null) return controls;
            return controls = new Controls();
        }
        set => controls = value;
    }

    public UserInputSettings()
    {
        InputEnabled = false;
        Controls.Disable();
    }
}

public class GameManager : NetworkBehaviour 
{
    [SerializeField]
    private float turnTime = 10f;
    [SerializeField]
    private float intervalTime = 2f;
    [SerializeField]
    private List<Worm> players = new List<Worm>();
    
    private int currentPlayerIndex = 0;

    public static GameManager instance;
    public readonly Dictionary<Worm, UserInputSettings> inputSettings = new Dictionary<Worm, UserInputSettings>();

    private bool hasTurnEnded = false;
    public UnityEvent onTurnEnded;

    private void Awake () => instance = this;
    
    #region Server

    [ContextMenu("Launch Game")]
    [Server]
    public void LaunchGame()
    {
        StartCoroutine(RunGame());
    }

    [Server]
    private bool ConditionForEndOfGameIsTrue()
    {
        // TODO
        return false;
    }

    [Server]
    public IEnumerator RunGame()
    {
        // temp variable
        int nbTurns = 3;

        // temp while, should be a while (true) at the end
        while (nbTurns >= 0)
        {
            foreach (Worm worm in players)
            {
                if (ConditionForEndOfGameIsTrue())
                    yield break; // TODO end of game
                // inputSettings[worm].Controls.Enable();
                worm.StartTurn();
                yield return new WaitForSeconds(turnTime);
                // inputSettings[worm].Controls.Disable();
                worm.EndTurn();
                yield return new WaitForSeconds(intervalTime);
            }
            
            // game logic
            
            
            nbTurns--;
        }
    }
    
    [Server]
    public void AddPlayer(Worm p)
    {
        if (false) // max nb players or party already running
            Debug.Log("<color=red>" + "Cannot add a player" + "</color>");
        players.Add(p);
        inputSettings[p] = new UserInputSettings();
    }


    [Server]
    public void RemovePlayer(Worm p)
    {
        if (inputSettings.ContainsKey(p))
            inputSettings.Remove(p);
        players.Remove(p);
    }
    
    #endregion
}
/*
#region Server
    
private void Awake()
{
    instance = this;
    onTurnEnded.AddListener(() => hasTurnEnded = true);
}

[ContextMenu("Launch Game")]
[ServerCallback]
private void LaunchGame()
{
    StartCoroutine(HandleTurn());
}


[Server]
private bool ConditionForEndOfGameIsTrue()
{
    // TODO
    return false;
}
    
[Server]
private IEnumerator HandleTurn()
{
    while (true)
    {
        if (ConditionForEndOfGameIsTrue() == true)
            yield break; // TODO notify end of game to all clients
        PlayTurn();
        yield return new WaitUntil(() => hasTurnEnded == true);
        hasTurnEnded = false;
        yield return new WaitForSeconds(intervalTime);
        currentPlayerIndex++;
        if (currentPlayerIndex >= players.Count)
            currentPlayerIndex = 0;
    }
}

[Server]
private void PlayTurn()
{
    Worm currentPlayer = players[currentPlayerIndex];

    if (currentPlayer.TryGetComponent(out NetworkIdentity netID))
    {
        currentPlayer.TargetPlayTurn(netID.connectionToClient, turnTime);
    }
}
    
[Server]
public void AddPlayer(Worm p)
{
    if (false) // max nb players or party already running
        Debug.Log("<color=red>" + "Cannot add a player" + "</color>");
    p.playColor = UnityEngine.Random.ColorHSV();
    players.Add(p);
    inputSettings[p] = new UserInputSettings();
    // p.OnTurnEnded += UpdateEndTurn;
}


[Server]
public void RemovePlayer(Worm p)
{
    if (inputSettings.ContainsKey(p))
        inputSettings.Remove(p);
    players.Remove(p);
}
    
#endregion

*/