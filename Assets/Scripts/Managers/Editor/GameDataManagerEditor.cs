using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Custom editor for GameDataManager
/// </summary>
[CustomEditor(typeof(GameDataManager))]
public class GameDataManagerEditor : Editor
{
    private GameDataManager gameDataManager; // GameDataManager.cs script

    int indexToLoad = 0;
    int createGameIndex = 0;
    int deleteGameIndex = 0;
    bool isHardcore;

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
            gameDataManager.gameManager.GameDatas = gameDataManager.LoadGamesFromFiles();
        }

        indexToLoad = EditorGUILayout.IntSlider("Index to Load", indexToLoad, 0, gameDataManager.gameManager.GameDatas.Length - 1);
        if (GUILayout.Button($"Load savegame_{indexToLoad}"))
        {
            gameDataManager.LoadGame(indexToLoad);
        }

        if (GUILayout.Button($"Save Game {gameDataManager.gameManager.LoadedGameData.index}"))
        {
            gameDataManager.SaveGame();
        }

        createGameIndex = EditorGUILayout.IntSlider("Create Game Index", createGameIndex, 0, gameDataManager.gameManager.GameDatas.Length - 1);
        isHardcore = EditorGUILayout.Toggle("Is Hardcore", isHardcore);
        if (GUILayout.Button($"Create game at savegame_{createGameIndex}"))
        {
            gameDataManager.CreateGame(createGameIndex, isHardcore ? Difficulty.HARDCORE : Difficulty.CASUAL);
        }

        deleteGameIndex = EditorGUILayout.IntSlider("Delete Game Index", deleteGameIndex, 0, gameDataManager.gameManager.GameDatas.Length - 1);
        if (GUILayout.Button($"Delete game at savegame_{deleteGameIndex}"))
        {
            gameDataManager.DeleteSave(deleteGameIndex);
        }
    }
}
