using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class GameManager : NetworkBehaviour 
{
    [SerializeField]
    private float turnTime = 10f;
    [SerializeField]
    private float intervalTime = 2f;
    [SerializeField]
    private List<Worm> players = new List<Worm>();
    
    public static GameManager instance;

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
                worm.StartTurn();
                yield return new WaitForSeconds(turnTime);
                worm.EndTurn();
                yield return new WaitForSeconds(intervalTime);
            }
            nbTurns--;
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
    }


    /// <summary>
    /// Remove a player from the list of player
    /// </summary>
    /// <param name="p">Player</param>
    [Server]
    public void RemovePlayer(Worm p)
    {
        players.Remove(p);
    }
    
    #endregion
}
