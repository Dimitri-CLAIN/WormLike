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

    public override void OnStartServer()
    {
        base.OnStartServer();
        Invoke(nameof(LaunchGame), 2f);
    }

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
        int playersAlive = 0;
        foreach (Worm worm in players)
        {
            if (worm.Health.HealthPoints > 0)
                playersAlive++;
        }
        if (playersAlive <= 1)
            return true;
        return false;
    }

    
    /// <summary>
    /// Runs the game logic on the server and interaction between players
    /// <para>Disconnect and go back to room oes not work SMH...</para>
    /// </summary>
    /// <returns>IEnum</returns>
    [Server]
    public IEnumerator RunGame()
    {
        while (true)
        {
            foreach (Worm worm in players)
            {
                if (ConditionForEndOfGameIsTrue())
                {
                    WormRoomManager wormRoomManager = NetworkRoomManager.singleton as WormRoomManager;
                    if (wormRoomManager != null)
                    {
                        // wormRoomManager.ReturnToRoomScene();
                        foreach (var players in NetworkServer.connections)
                        {
                            if (players.Value.identity.isClientOnly)
                                wormRoomManager.StopClient();
                            else
                                wormRoomManager.StopHost();
                        }
                    }
                    
                }
                else if (worm.Health.HealthPoints <= 0)
                    continue;
                worm.StartTurn(turnTime);
                playerTurnCoroutine = StartCoroutine(RunTurnCountdown(turnTime));
                yield return RunTurnCountdown(turnTime);
                worm.EndTurn();
                yield return new WaitForSeconds(intervalTime);
            }
        }
    }
    
    
    /// <summary>
    /// Countdown that can be interrupted whenever the <c>shouldStopTurn</c> is true
    /// </summary>
    /// <param name="turn">Number of seconds to count down</param>
    /// <returns>IEnumerator</returns>
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


    /// <summary>
    /// Sets the <c>shouldStopTurn</c> to true in this instance
    /// </summary>
    [Server]
    private void EndPlayerTurn() => shouldStopTurn = true;
    
    #endregion
}
