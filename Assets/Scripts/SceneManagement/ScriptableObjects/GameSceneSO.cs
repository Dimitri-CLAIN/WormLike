using Mirror;
using UnityEngine;
using UnityEngine.Serialization;

public class GameSceneSO : DescriptionBaseSO
{
    public GameSceneType sceneType;

    [Header("Scene Management")]
    [Scene]
    [FormerlySerializedAs("m_OfflineScene")]
    public string scenePath;
    // public AudioCueSO musicTrack;

    public enum GameSceneType
    {
        //Playable scenes
        Location, //SceneSelector tool will also load PersistentManagers and Gameplay
        Menu, //SceneSelector tool will also load Gameplay

        //Special scenes
        Initialisation,
        PersistentManagers,
        Gameplay,
    }
}