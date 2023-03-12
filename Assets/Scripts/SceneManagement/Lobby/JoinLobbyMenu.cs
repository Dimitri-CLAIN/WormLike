using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DapperDino.Mirror.Tutorials.Lobby
{
    public class JoinLobbyMenu : MonoBehaviour
    {
        [SerializeField] private NetworkManagerLobby _networkManager = null;

        [Header("UI")]
        [SerializeField] private GameObject _landingPagePanel = null;
        [SerializeField] private TMP_InputField _ipAddressInputField = null;
        [SerializeField] private Button _joinButton = null;

        private void OnEnable()
        {
            NetworkManagerLobby.OnClientConnected += HandleClientConnected;
            NetworkManagerLobby.OnClientDisconnected += HandleClientDisconnected;
        }

        private void OnDisable()
        {
            NetworkManagerLobby.OnClientConnected -= HandleClientConnected;
            NetworkManagerLobby.OnClientDisconnected -= HandleClientDisconnected;
        }

        public void JoinLobby()
        {
            string ipAddress = _ipAddressInputField.text;

            _networkManager.networkAddress = ipAddress;
            _networkManager.StartClient();

            _joinButton.interactable = false;
        }

        private void HandleClientConnected()
        {
            _joinButton.interactable = true;

            gameObject.SetActive(false);
            _landingPagePanel.SetActive(false);
        }

        private void HandleClientDisconnected()
        {
            _joinButton.interactable = true;
        }
    }
}