using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private NetworkManagerLobby _networkManager = null;

    [Header("UI")]
    [SerializeField] public GameObject _landingPagePanel = null;

    public void HostLobby()
    {
        _networkManager.StartHost();
        _landingPagePanel.SetActive(false);
    }
}