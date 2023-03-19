using Mirror;
using UnityEngine;

public class NetworkGamePlayerLobby : NetworkBehaviour
{
    [SyncVar]
    private string _displayName = "Loading...";

    [SerializeField] public Worm Worm = default;

    private NetworkManagerLobby _room;
    private NetworkManagerLobby Room
    {
        get
        {
            if (_room != null) { return _room; }
            return _room = NetworkManager.singleton as NetworkManagerLobby;
        }
    }

    public override void OnStartClient()
    {
        DontDestroyOnLoad(gameObject);

        Room.GamePlayers.Add(this);
    }

    public override void OnStopClient()
    {
        Room.GamePlayers.Remove(this);
    }

    [Server]
    public void SetDisplayName(string displayName)
    {
        _displayName = displayName;
    }
}