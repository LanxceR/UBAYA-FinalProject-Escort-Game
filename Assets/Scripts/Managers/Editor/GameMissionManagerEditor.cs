using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Custom editor for GameMissionManager
/// </summary>
[CustomEditor(typeof(GameMissionManager)), CanEditMultipleObjects]
public class GameMissionManagerEditor : Editor
{
    private GameMissionManager gameMissionManager; // GameDataManager.cs script

    int indexToLoad = 0;

    // Draws the GUI on Unity Editor Inspector
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        // Fetch the target object that is being inspected in inspector
        gameMissionManager = (GameMissionManager)target;

        if (GUILayout.Button("Generate Missions"))
        {
            gameMissionManager.GenerateMissions(gameMissionManager.gameManager.LoadedGameData.daysPassed);
        }

        indexToLoad = EditorGUILayout.IntSlider("Index to Load", indexToLoad, 0, gameMissionManager.gameManager.MissionDatas.Length - 1);
        if (GUILayout.Button($"Load Mission index = {indexToLoad}"))
        {
            gameMissionManager.LoadMission(indexToLoad);
        }
    }
}
