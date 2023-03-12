using UnityEngine;
using UnityEngine.SceneManagement;

public class InitializationLoader : MonoBehaviour
{
    [SerializeField] private GameSceneSO _menuToLoad = default;
    
    [SerializeField] private GameObject[] _persistentObjects;

    void Start()
    {
        SceneManager.LoadSceneAsync(_menuToLoad.scenePath, LoadSceneMode.Additive);
        foreach (var persistentObject in _persistentObjects)
        {
            if (persistentObject)
            {
                DontDestroyOnLoad(persistentObject);
            }
        }
    }
}
