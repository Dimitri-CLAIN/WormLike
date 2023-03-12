using System;
using System.Collections;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader: NetworkManager
{
    [SerializeField] private GameSceneSO _gameplayScene = default;
    // [SerializeField] private GameSceneSO _inputReader = default;
    
    [Header("Listening to")]
    [SerializeField] private LoadEventChannelSO _loadLocation = default;
    [SerializeField] private LoadEventChannelSO _loadMenu = default;
    [SerializeField] private LoadEventChannelSO _coldStartupLocation = default;
    
    // [SerializeField] private BoolEventChannelSO _toggleLoadingScreen = default;
    [SerializeField] private VoidEventChannelSO _onSceneReady = default;
    // [SerializeField] private FadeChannelSO _fadeRequestChannel = default;

    private GameSceneSO _sceneToLoad;
    private GameSceneSO _currentlyLoadedScene;
    private Scene _gameplayManagerSceneInstance = new Scene(); // Une instance de la scene

    private bool _showLoadingScreen;

    private void OnEnable()
    {
        _loadLocation.OnLoadingRequested += LoadLocation;
        _loadMenu.OnLoadingRequested += LoadMenu;
#if UNITY_EDITOR
        _coldStartupLocation.OnLoadingRequested += LocationColdStartup;
#endif
    }

    private void OnDisable()
    {
        _loadLocation.OnLoadingRequested -= LoadLocation;
        _loadMenu.OnLoadingRequested -= LoadMenu;
#if UNITY_EDITOR
        _coldStartupLocation.OnLoadingRequested -= LocationColdStartup;
#endif
    }

#if UNITY_EDITOR
    private void LocationColdStartup(GameSceneSO currentlyOpenedLocation, bool showLoadingScreen, bool fadeScreen)
    {
        _currentlyLoadedScene = currentlyOpenedLocation;

        if (_currentlyLoadedScene.sceneType == GameSceneSO.GameSceneType.Location)
        {
            SceneManager.LoadSceneAsync(_gameplayScene.scenePath);
            StartGameplay();
        }
    }
#endif

    private void LoadLocation(GameSceneSO locationToLoad, bool showLoadingScreen, bool fadeScreen)
    {
        if (NetworkServer.isLoadingScene)
            return;
        _showLoadingScreen = showLoadingScreen;
        ServerChangeScene(locationToLoad.scenePath);
    }

    private void LoadMenu(GameSceneSO menuToLoad, bool showLoadingScreen, bool fadeScreen)
    {
        if (NetworkServer.isLoadingScene)
            return;
        _showLoadingScreen = showLoadingScreen;
        if (_gameplayManagerSceneInstance.isLoaded)
            SceneManager.UnloadSceneAsync(_gameplayManagerSceneInstance.name);
        ServerChangeScene(menuToLoad.scenePath);
    }

 //    void FinishLoadScene()
 //    {
 //        base.FinishLoadScene();
 //        LightProbes.TetrahedralizeAsync();
 //
 //        if (_showLoadingScreen)
	// 		_toggleLoadingScreen.RaiseEvent(false);
 //
 //        StartGameplay();
	// }

	private void StartGameplay()
	{
		_onSceneReady.RaiseEvent();
	}

	private void ExitGame()
	{
		Application.Quit();
		Debug.Log("Exit!");
	}
}