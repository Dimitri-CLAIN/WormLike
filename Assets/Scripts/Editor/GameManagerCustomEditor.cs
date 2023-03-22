
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameManager))]
public class GameManagerCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        DrawDefaultInspector();
        
        GUILayout.Space(20);

        if (GUILayout.Button("Launch Game"))
        {
            GameManager obj = target as GameManager;
            
            if (obj != null)
                obj.LaunchGame();
        }
    }
}
