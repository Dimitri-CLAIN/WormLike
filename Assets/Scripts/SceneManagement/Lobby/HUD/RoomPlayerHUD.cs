using Mirror;
using TMPro;
using UnityEngine;

public class RoomPlayerHUD: NetworkBehaviour
{
    [Header("GUI")]
    [SerializeField] public GameObject editButtons = default;
    [SerializeField] public TMP_Text playerReadyText = default;
    [SerializeField] public TMP_Text nameText;

    [Header("Utils")]
    [SyncVar(hook = nameof(OnDisplayNameChanged))]
    public string DisplayName = "Waiting...";
    [SyncVar(hook = nameof(OnChangeChampion))]
    private int _championIndex;

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

    public void OnDisplayNameChanged(string oldName, string newName)
    {
        nameText.text = newName;
    }

    public void OnChangeChampion(int oldIndex, int newIndex)
    {
        championes[oldIndex].SetActive(false);
        championes[newIndex].SetActive(true);
    }

    [Command]
    public void CmdChangeUpChampion()
    {
        int newChampionIndex = _championIndex + 1;

        if (newChampionIndex >= championes.Length) {
            _championIndex = 0;
        } else {
            _championIndex = newChampionIndex;
        }
    }

    [Command]
    public void CmdChangeDownChampion()
    {
        int newChampionIndex = _championIndex - 1;

        if (newChampionIndex < 0) {
            _championIndex = championes.Length - 1;
        } else {
            _championIndex = newChampionIndex;
        }
    }
}