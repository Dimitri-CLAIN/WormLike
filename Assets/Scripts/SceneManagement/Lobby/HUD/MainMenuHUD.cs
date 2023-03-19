using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuHUD : MonoBehaviour
{
    public Button buttonHost, buttonJoin;
    public TMP_InputField inputFieldAddress;

    private void Start()
    {
        if (NetworkManager.singleton.networkAddress != "localhost") { inputFieldAddress.text = NetworkManager.singleton.networkAddress; }

        inputFieldAddress.onValueChanged.AddListener(delegate { ValueChangeCheck(); });

        buttonHost.onClick.AddListener(ButtonHost);
        buttonJoin.onClick.AddListener(ButtonJoin);
    }

    public void ValueChangeCheck()
    {
        NetworkManager.singleton.networkAddress = inputFieldAddress.text;
    }

    public void ButtonHost()
    {
        NetworkManager.singleton.StartHost();
    }
    
    public void ButtonJoin()
    {
        NetworkManager.singleton.StartClient();
    }
}