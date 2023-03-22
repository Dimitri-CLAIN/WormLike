using Mirror;
using Mirror.Examples.Chat;
using TMPro;
using UnityEngine;

public class RoomPlayerHUD: NetworkBehaviour
{
    [SerializeField]
    private NetworkRoomPlayer player;
    
    [Header("GUI")]
    [SerializeField] private GameObject playerSlot = default;
    [SerializeField] private TMP_Text playerNameText = default;
    [SerializeField] private TMP_Text playerReadyText = default;

    public void OnReadyChange()
    {
         playerReadyText.text = player.readyToBegin ?
            "<color=green>Ready</color>" :
            "<color=red>Not Ready</color>";
    }
}