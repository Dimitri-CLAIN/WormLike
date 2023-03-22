using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyHUD : NetworkBehaviour
{
    public Button buttonStop;
    
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
    
    public void ButtonStop()
    {
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopHost();
        }
        else if (NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopClient();
        }
    }
    
    private bool _isLeader;
    public bool IsLeader
    {
        set
        {
            _isLeader = value;
            _startGameButton.gameObject.SetActive(value);
        }
    }

    private RoomManager room;
    private RoomManager Room
    {
        get
        {
            if (room != null) { return room; }
            return room = RoomManager.singleton as RoomManager;
        }
    }

    private void OnDestroy()
    {
        _backGameButton.onClick.RemoveAllListeners();
    }

    public override void OnStartAuthority()
    {
        if (!isOwned) { return; }

        CmdSetDisplayName(PlayerNameInput.DisplayName);
        onNameChanged.OnEventRaised += CmdSetDisplayName;
    }
    
    public override void OnStartClient()
    {
        SetupBasicDisplay();
        UpdateDisplay();
    }
    
    public override void OnStopClient()
    {
        UpdateDisplay();
    }

    public void HandleReadyStatusChanged(bool oldValue, bool newValue) => UpdateDisplay();
    public void HandleDisplayNameChanged(string oldValue, string newValue) => UpdateDisplay();

    private void SetupBasicDisplay()
    {
        if (Room.roomSlots.Count == 0) { return; }
        
        int i = room.roomSlots.FindIndex(p => p.isOwned);
        if (i == -1 || i > _playerSlots.Length) { return; }

        _ipText.text = room.networkAddress;
        _playerSlots[i]?.SetActive(true);
    }
    private void UpdateDisplay()
    {
        for (int i = 0; i < _playerNameTexts.Length; i++)
        {
            _playerNameTexts[i].text = "Waiting For Player...";
            _playerReadyTexts[i].text = string.Empty;
        }

        for (int i = 0; i < room.roomSlots.Count; i++)
        {
            _playerNameTexts[i].text = room.roomSlots[i].name;
            _playerReadyTexts[i].text = room.roomSlots[i].readyToBegin ?
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

        room.ReadyStatusChanged();
    }

    [Command]
    public void CmdStartGame()
    {
        room.Start();
    }
}