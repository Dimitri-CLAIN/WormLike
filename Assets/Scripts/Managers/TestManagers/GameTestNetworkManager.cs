
using System;
using KawaiiImplementation;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameTestNetworkManager : NetworkManager
{
    [SerializeField]
    private GameObject gameSystem;

    [SerializeField]
    public KawaiiSlimeSelector slimeSelector;
    

    public event Action OnServerStopped;

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);
        if (conn.identity.TryGetComponent<Worm>(out Worm player))
        {
            player.playColor = Random.ColorHSV();

            
            // KawaiiSlimeSelector.KawaiiSlime slimeType = (KawaiiSlimeSelector.KawaiiSlime)Random.Range(0, 8);
            // player.slimeType = slimeType;
            // GameObject slimeGO = slimeSelector.SelectSlime(slimeType);
            // // NetworkServer.Spawn(slimeGO, conn);
            // player.RpcSetSlime(slimeGO, slimeType);
            
            // KawaiiSlimeSelector.KawaiiSlime slimeType = (KawaiiSlimeSelector.KawaiiSlime)Random.Range(0, 8);
            // player.slimeType = slimeType;
            // var slimeGO = slimeSelector.SelectSlime(slimeType);
            // player.slime = Instantiate(slimeGO, player.SlimeHolder);
            // NetworkServer.Spawn(player.slime, conn);

            player.slimeType = (KawaiiSlimeSelector.KawaiiSlime)Random.Range(0, 8);
            
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
}
