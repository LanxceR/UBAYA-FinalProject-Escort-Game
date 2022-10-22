using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Custome ditor for GameDataManager
/// </summary>
[CustomEditor(typeof(GameDataManager))]
public class GameDataManagerEditor : Editor
{
    private GameDataManager gameDataManager; // GameDataManager.cs script

    // Draws the GUI on Unity Editor Inspector
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        // Fetch the target object that is being inspected in inspector
        gameDataManager = (GameDataManager)target;

        if (GUILayout.Button("Save All"))
        {
            gameDataManager.SaveGamesToFiles();
        }

        if (GUILayout.Button("Load All"))
        {
            gameDataManager.gameManager.PlayerDatas = gameDataManager.LoadGamesFromFiles();
        }
    }
}
