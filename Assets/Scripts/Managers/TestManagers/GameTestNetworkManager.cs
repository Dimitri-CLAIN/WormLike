
using Mirror;
using UnityEngine;

public class GameTestNetworkManager : NetworkManager
{
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);
        if (conn.identity.TryGetComponent<MyPlayer>(out MyPlayer player))
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
        if (conn.identity.TryGetComponent<MyPlayer>(out MyPlayer player))
        {
            if (GameManager.instance == null)
            {
                Debug.Log("<color=red>" + "Error : No Game Manager was found" + "</color>");
                return;
            }
            GameManager.instance.RemovePlayer(player);
        }
    }
}
