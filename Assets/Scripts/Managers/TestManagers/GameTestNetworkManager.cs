
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
}
