using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class GameManager : NetworkBehaviour 
{
    [SerializeField]
    private int turnTime = 10;
    [SerializeField]
    private int intervalTime = 2;
    [SerializeField]
    private List<Worm> players = new List<Worm>();
    
    public static GameManager instance;

    Coroutine playerTurnCoroutine;
    private bool shouldStopTurn = false;

    private void Awake () => instance = this;
    
    #region Server

    /// <summary>
    /// Temporary a ContextMenu method to launch the game from the Unity Inspector
    /// </summary>
    [ContextMenu("Launch Game")]
    [Server]
    public void LaunchGame()
    {
        StartCoroutine(RunGame());
    }

    
    /// <summary>
    /// Determines if the game is finished or not
    /// </summary>
    /// <returns><c>True</c> if the game ends</returns>
    [Server]
    private bool ConditionForEndOfGameIsTrue()
    {
        // TODO
        return false;
    }

    
    /// <summary>
    /// Runs the game logic on the server and interaction between players
    /// </summary>
    /// <returns>IEnum</returns>
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
                worm.StartTurn(turnTime);
                playerTurnCoroutine = StartCoroutine(RunTurnCountdown(turnTime));
                yield return RunTurnCountdown(turnTime);
                Debug.Log($"<color=blue>Turn ended</color>");
                // yield return new WaitForSeconds(turnTime);
                worm.EndTurn();
                yield return new WaitForSeconds(intervalTime);
            }
            nbTurns--;
        }
    }
    
    
    [Server]
    private IEnumerator RunTurnCountdown(int turn)
    {
        float timer = turn;
        shouldStopTurn = false;
        
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            if (shouldStopTurn == true)
            {
                yield break;
            }
            yield return null;
        }
    }
    
    /// <summary>
    /// Add a player to the list of player
    /// </summary>
    /// <param name="p">Player</param>
    [Server]
    public void AddPlayer(Worm p)
    {
        if (false) // TODO max nb players or party already running
            Debug.Log("<color=red>" + "Cannot add a player" + "</color>");
        players.Add(p);
        p.Weapon.OnShotTriggered += EndPlayerTurn;
        p.Canvas.OnButtonEndTurnPressed += EndPlayerTurn;
    }


    /// <summary>
    /// Remove a player from the list of player
    /// </summary>
    /// <param name="p">Player</param>
    [Server]
    public void RemovePlayer(Worm p)
    {
        players.Remove(p);
        p.Weapon.OnShotTriggered -= EndPlayerTurn;
        p.Canvas.OnButtonEndTurnPressed -= EndPlayerTurn;
    }


    [Server]
    private void EndPlayerTurn() => shouldStopTurn = true;
    
    #endregion
}
