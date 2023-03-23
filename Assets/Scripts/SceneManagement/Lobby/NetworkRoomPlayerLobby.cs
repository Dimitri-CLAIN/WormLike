using TMPro;
using Mirror;
using Unity;
using UnityEngine;
using UnityEngine.UI;

public class NetworkRoomPlayerLobby : NetworkBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject _lobbyUI = null;

    [SerializeField] private TMP_Text _ipText = default;
    [SerializeField] private GameObject[] _playerSlots = new GameObject[4];
    [SerializeField] private TMP_Text[] _playerNameTexts = new TMP_Text[4];
    [SerializeField] private TMP_Text[] _playerReadyTexts = new TMP_Text[4];
    [SerializeField] private Button _startGameButton = null;
    [SerializeField] private Button _backGameButton = default;
    
    [SyncVar(hook = nameof(HandleDisplayNameChanged))]
    public string DisplayName = "Waiting...";
    [SyncVar(hook = nameof(HandleReadyStatusChanged))]
    public bool IsReady = false;
    
    [Header("Listen")]
    [SerializeField] private StringEventChannelSO onNameChanged;

    private bool _isLeader;
    public bool IsLeader
    {
        set
        {
            _isLeader = value;
            _startGameButton.gameObject.SetActive(value);
        }
    }

    private NetworkManagerLobby room;
    private NetworkManagerLobby Room
    {
        get
        {
            if (room != null) { return room; }
            return room = NetworkManager.singleton as NetworkManagerLobby;
        }
    }

    private void OnDestroy()
    {
        _backGameButton.onClick.RemoveAllListeners();
    }

    public void SetupBackButton()
    {
        _backGameButton.onClick.AddListener(() =>
            {
                // GameObject.Find("MainMenu")!.GetComponent<MainMenu>()!._landingPagePanel.SetActive(true);
                if (_isLeader) { NetworkManager.singleton.StopHost(); }
                else { NetworkManager.singleton.StopClient(); }
                Destroy(this);
            });
    }
    
    public override void OnStartAuthority()
    {
        if (!isOwned) { return; }

        CmdSetDisplayName(PlayerNameInput.DisplayName);
        onNameChanged.OnEventRaised += CmdSetDisplayName;
        _lobbyUI.SetActive(true);
    }

    public override void OnStartClient()
    {
        Room.RoomPlayers.Add(this);

        SetupBackButton();
        SetupBasicDisplay();
        UpdateDisplay();
    }

    public override void OnStopClient()
    {
        Room.RoomPlayers.Remove(this);

        UpdateDisplay();
    }

    public void HandleReadyStatusChanged(bool oldValue, bool newValue) => UpdateDisplay();
    public void HandleDisplayNameChanged(string oldValue, string newValue) => UpdateDisplay();

    private void SetupBasicDisplay()
    {
        if (Room.RoomPlayers.Count == 0) { return; }

        int i = Room.RoomPlayers.FindIndex(p => p.isOwned);
        if (i == -1 || i > _playerSlots.Length) { return; }

        _ipText.text = Room.networkAddress;
        _playerSlots[i]?.SetActive(true);
    }
    private void UpdateDisplay()
    {
        if (!isOwned)
        {
            foreach (var player in Room.RoomPlayers)
            {
                if (player.isOwned)
                {
                    player.UpdateDisplay();
                    break;
                }
            }

            return;
        }

        for (int i = 0; i < _playerNameTexts.Length; i++)
        {
            _playerNameTexts[i].text = "Waiting For Player...";
            _playerReadyTexts[i].text = string.Empty;
        }

        for (int i = 0; i < Room.RoomPlayers.Count; i++)
        {
            _playerNameTexts[i].text = Room.RoomPlayers[i].DisplayName;
            _playerReadyTexts[i].text = Room.RoomPlayers[i].IsReady ?
                "<color=green>Ready</color>" :
                "<color=red>Not Ready</color>";
        }
    }

    public void HandleReadyToStart(bool readyToStart)
    {
        if (!_isLeader) { return; }

        _startGameButton.interactable = readyToStart;
    }

    [Command]
    private void CmdSetDisplayName(string displayName)
    {
        DisplayName = displayName;
    }

    [Command]
    public void CmdReadyUp()
    {
        IsReady = !IsReady;

        Room.NotifyPlayersOfReadyState();
    }

    [Command]
    public void CmdStartGame()
    {
        if (Room.RoomPlayers[0].connectionToClient != connectionToClient) { return; }

        Room.StartGame();
    }
}