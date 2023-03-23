using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyHUD : NetworkBehaviour
{
    public static LobbyHUD instance;
    public Button buttonStop;
    
    [SerializeField]
    private LayoutGroup layoutSlots;

    public LayoutGroup LayoutSlots => layoutSlots;

    [SerializeField] public TMP_Text ipText = default;
    
    [SerializeField] public Button backGameButton = default;
    [SerializeField] public Button readyGameButton = default;
    [SerializeField] public InputField nameInput = default;

    private void Awake() => instance = this;
    public void ButtonStop()
    {
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            Room.StopHost();
        }
        else if (NetworkClient.isConnected)
        {
            Room.StopClient();
        }
    }
    
    private bool _isLeader;
    public bool IsLeader
    {
        set
        {
            _isLeader = value;
        }
    }

    private WormRoomManager room;
    private WormRoomManager Room
    {
        get
        {
            if (room != null) { return room; }
            return room = WormRoomManager.singleton as WormRoomManager;
        }
    }

    private void OnDestroy()
    {
        readyGameButton.onClick.RemoveAllListeners();
    }

    public override void OnStartAuthority()
    {
        if (!isOwned) { return; }

    }

    private void SetupBasicDisplay()
    {
        if (Room.roomSlots.Count == 0) { return; }

        // _ipText.text = Room.networkAddress;
    }
}