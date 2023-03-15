
using System;
using Mirror;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameTestNetworkManager : NetworkManager
{
    [SerializeField]
    private GameObject gameSystem;

    public event Action OnServerStopped;

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);
        if (conn.identity.TryGetComponent<Worm>(out Worm player))
        {
            player.playColor = Random.ColorHSV();
            if (GameManager.instance == null)
            {
                Debug.Log("<color=red>" + "Error : No Game Manager was found" + "</color>");
                return;
            }
            GameManager.instance.AddPlayer(player);
        }
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        /// TODO fix deletion of Player in the GameManager
        /// the Game Manager iterates thru players with a foreach, we should update this if necessary
        /// I don't think it is updated when a list is
        if (conn.identity.TryGetComponent<Worm>(out Worm player))
        {
            if (GameManager.instance == null)
            {
                Debug.Log("<color=red>" + "Error : No Game Manager was found" + "</color>");
                return;
            }
            GameManager.instance.RemovePlayer(player);
        }
    }


    public override void OnStopServer()
    {
        OnServerStopped?.Invoke();
    }
}
