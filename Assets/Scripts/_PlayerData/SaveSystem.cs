using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

/// <summary>
/// Static class for saving and loading savegames/playerdata
/// </summary>
public static class SaveSystem
{
    /*  PERSISTENT DATA PATH lists for various OS:
     *  Windows Store Apps: Application.persistentDataPath points to %userprofile%\AppData\Local\Packages\<productname>\LocalState
     *  Windows Editor and Standalone Player: Application.persistentDataPath usually points to %userprofile%\AppData\LocalLow\<companyname>\<productname>
     *  WebGL: Application.persistentDataPath points to /idbfs/<md5 hash of data path> where the data path is the URL stripped of everything including and after the last '/' before any '?' components.
     *  Linux: Application.persistentDataPath points to $XDG_CONFIG_HOME/unity3d or $HOME/.config/unity3d
     *  iOS: Application.persistentDataPath points to /var/mobile/Containers/Data/Application/<guid>/Documents
     *  tvOS: Application.persistentDataPath is not supported and returns an empty string
     *  Android: Application.persistentDataPath points to /storage/emulated/0/Android/data/<packagename>/files on most devices (some older phones might point to location on SD card if present), the path is resolved using android.content.Context.getExternalFilesDir
     *  Mac: Application.persistentDataPath points to the user Library folder. (This folder is often hidden.) In recent Unity releases user data is written into ~/Library/Application Support/company name/product name. Older versions of Unity wrote into the ~/Library/Caches folder, or ~/Library/Application Support/unity.company name.product name. These folders are all searched for by Unity. The application finds and uses the oldest folder with the required data on your system
     *  Read more at https://docs.unity3d.com/ScriptReference/Application-persistentDataPath.html
     */

    // TODO: Maybe implement a different serialization method (other than binaryformatter)?

    // Save the game
    internal static void SaveGame(string saveName,
        int saveNum,
        Difficulty difficulty,
        float money,
        int daysPassed,
        int missionsCompleted,
        int missionsFailed)
    {
        // Prepare a playerData object
        PlayerData savegame = new PlayerData(saveNum, difficulty, money, daysPassed, missionsCompleted, missionsFailed);

        // Setup a BinaryFormatter object
        BinaryFormatter formatter = new BinaryFormatter();
        // Setup the filepath location
        string path = Path.Combine(Application.persistentDataPath, $"{saveName}.save");
        // Create a new filestream to create a savefile (or overwrite if one already exists)
        using FileStream stream = new FileStream(path, FileMode.Create);


        // Write data into a file, then close stream
        formatter.Serialize(stream, savegame);
        stream.Close();

        Debug.Log($"File {saveName} written in {path}");
    }
    internal static void SaveGame(string saveName, PlayerData playerData)
    {
        // Prepare a playerData object
        PlayerData savegame = playerData;

        // Setup a BinaryFormatter object
        BinaryFormatter formatter = new BinaryFormatter();
        // Setup the filepath location
        string path = Path.Combine(Application.persistentDataPath, $"{saveName}.save");
        // Create a new filestream to create a savefile (or overwrite if one already exists)
        using FileStream stream = new FileStream(path, FileMode.Create);


        // Write data into a file, then close stream
        formatter.Serialize(stream, savegame);
        stream.Close();

        Debug.Log($"File {saveName} written in {path}");
    }

    // Load save file
    public static PlayerData LoadGame(string saveName)
    {
        // Setup the filepath location
        string path = Path.Combine(Application.persistentDataPath, $"{saveName}.save");

        try
        {
            // Look for file
            if (File.Exists(path))
            {
                // Setup a BinaryFormatter object
                BinaryFormatter formatter = new BinaryFormatter();
                // Create a new filestream to open a savefile
                FileStream stream = new FileStream(path, FileMode.Open);

                // Read data from file, then close stream
                PlayerData savegame = formatter.Deserialize(stream) as PlayerData;
                stream.Close();

                string debug = $"{saveName} Loaded! \n" +
                               $"Index: {savegame.index} \n" +
                               $"Difficulty: {savegame.difficulty} \n" +
                               $"Money: {savegame.money} \n" +
                               $"Days Passed: {savegame.daysPassed} \n" +
                               $"Missions Completed: {savegame.missionsCompleted} \n" +
                               $"Missions Failed: {savegame.missionsFailed} \n";
                Debug.Log(debug);
                return savegame;
            }
            else
            {
                Debug.LogWarning($"No {saveName}.save file found in {path}");
                return null;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load save file \n" + e);
            return null;
        }        
    }

    // Delete a save file
    public static bool DeleteSave(string saveName)
    {
        // Setup the filepath location
        string path = Path.Combine(Application.persistentDataPath, $"{saveName}.save");

        try
        {
            // Look for file
            if (File.Exists(path))
            {
                File.Delete(path);

                Debug.Log($"{saveName}.save file found in {path} has been deleted");
                return true;
            }
            else
            {
                Debug.LogWarning($"No {saveName}.save file found in {path}");
                return false;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to delete save file \n" + e);
            return false;
        }
    }
}
