using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomPlayerHUD: NetworkBehaviour
{
    [Header("GUI")]
    [SerializeField] public GameObject editButtons = default;
    [SerializeField] public TMP_Text playerReadyText = default;
    [SerializeField] public TMP_Text nameText;
    [SerializeField] public Button rightEditButton;
    [SerializeField] public Button leftEditButton;

    [Header("Utils")]
    [SyncVar(hook = nameof(OnDisplayNameChanged))]
    public string DisplayName = "Waiting...";
    [SyncVar(hook = nameof(OnChampionChanged))]
    private int _championIndex = 0;

    [SerializeField] public GameObject[] championes;
    
    [Header("Listen")]
    [SerializeField] private StringEventChannelSO onNameChanged;

    private void Awake()
    {
        onNameChanged.OnEventRaised += CmdSetDisplayName;
    }

    public void OnReadyChange(bool oldReadyState, bool newReadyState)
    {
        playerReadyText.text = newReadyState ?
            "<color=green>Ready</color>" :
            "<color=red>Not Ready</color>";
        LobbyHUD.instance.readyGameButton.GetComponentInChildren<TMP_Text>().text = !newReadyState ? "Ready" : "Cancel";
    }

    [Command]
    private void CmdSetDisplayName(string newName)
    {
        DisplayName = newName;
    }

    [Client]
    public void OnDisplayNameChanged(string oldName, string newName)
    {
        nameText.text = DisplayName;
    }

    [Client]
    public void OnChampionChanged(int oldIndex, int newIndex)
    {
        championes[oldIndex].SetActive(false);
        championes[newIndex].SetActive(true);
    }
}