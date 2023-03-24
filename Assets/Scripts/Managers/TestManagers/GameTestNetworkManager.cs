
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
            
            KawaiiSlimeSelector.KawaiiSlime slimeType = (KawaiiSlimeSelector.KawaiiSlime)Random.Range(0, 8);
            player.slimeType = slimeType;
            var slimeGO = slimeSelector.SelectSlime(slimeType);
            player.slime = Instantiate(slimeGO, player.SlimeHolder);
            NetworkServer.Spawn(player.slime, conn);

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


    public override void OnClientConnect()
    {
        base.OnClientConnect();
        Debug.Log($"<color=green>on client connect</color>");

        // var connections = NetworkServer.connections;
        // foreach (var pairConn in connections)
        // {
        //     Worm worm = pairConn.Value.identity.GetComponent<Worm>();
        //     if (worm.slime == null)
        //     {
        //         Debug.Log($"<color=red>worm.slime is null</color>");
        //         KawaiiSlimeSelector.KawaiiSlime slimeType = worm.slimeType;
        //         worm.slime = Instantiate(slimeSelector.SelectSlime(slimeType), worm.SlimeHolder);
        //     } else
        //     {
        //         if (worm.SlimeHolder.GetChild(0) == null)
        //         {
        //             Debug.Log($"<color=yellow>slime holder has no child 0</color>");
        //             worm.slime.transform.SetParent(worm.SlimeHolder);
        //         }
        //     }
        //
        // }
        var slimeObjects = GameObject.FindGameObjectsWithTag("Slime");
        var playerObjects = GameObject.FindGameObjectsWithTag("Player");

        Debug.Log($"<color=grey>slime obj {slimeObjects.Length} || player obj {playerObjects.Length}</color>");

        foreach (var slimeObject in slimeObjects)
        {
            var animateSlime = slimeObject.GetComponent<AnimateSlime>();
            var slimeType = animateSlime.slimeType;

            foreach (var playerObject in playerObjects)
            {
                var worm = playerObject.GetComponent<Worm>();
                if (worm.slimeType != slimeType || worm.SlimeHolder.childCount != 0) continue;
                slimeObject.transform.SetParent(worm.SlimeHolder);
            }
        }
    }


    public override void OnClientSceneChanged()
    {
        base.OnClientSceneChanged();
        
        var slimeObjects = GameObject.FindGameObjectsWithTag("Slime");
        var playerObjects = GameObject.FindGameObjectsWithTag("Player");

        Debug.Log($"<color=grey>slime obj {slimeObjects.Length} || player obj {playerObjects.Length}</color>");

        foreach (var slimeObject in slimeObjects)
        {
            var animateSlime = slimeObject.GetComponent<AnimateSlime>();
            var slimeType = animateSlime.slimeType;

            foreach (var playerObject in playerObjects)
            {
                var worm = playerObject.GetComponent<Worm>();
                if (worm.slimeType != slimeType || worm.SlimeHolder.childCount != 0) continue;
                slimeObject.transform.SetParent(worm.SlimeHolder);
            }
        }
    }

    public override void OnStopServer()
    {
        OnServerStopped?.Invoke();
    }
}
