using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(NetworkIdentity))]
public class PlayerNameInput : NetworkBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_InputField nameInputField = default;
    [SerializeField] private Button continueButton = default;

    [Header("Channel")]
    [SerializeField] private StringEventChannelSO onNameChanged;

    public static string DisplayName { get; private set; }
    private const string PlayerPrefsNameKey = "PlayerName";

    private void Start() => SetUpInputField();

    [Client]
    private void SetUpInputField()
    {
        if (!PlayerPrefs.HasKey(PlayerPrefsNameKey)) { return; }

        string defaultName = PlayerPrefs.GetString(PlayerPrefsNameKey);

        nameInputField.text = defaultName;
        DisplayName = nameInputField.text;
        onNameChanged.RaiseEvent(DisplayName);

        SetPlayerName(defaultName);
    }

    public void SetPlayerName(string name)
    {
        if (!isLocalPlayer) { return; }
        continueButton.interactable = !string.IsNullOrEmpty(name);
    }

    public void CmdSavePlayerName()
    {
        if (!isLocalPlayer) { return; }
        DisplayName = nameInputField.text;

        PlayerPrefs.SetString(PlayerPrefsNameKey, DisplayName);
        onNameChanged.RaiseEvent(DisplayName);
    }
}