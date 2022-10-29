using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Custom editor for PlayerData
/// </summary>
[CustomEditor(typeof(PlayerData))]
public class PlayerDataEditor : Editor
{
    private PlayerData playerData; // PlayerData.cs script

    // Draws the GUI on Unity Editor Inspector
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        /*
        // Fetch the target object that is being inspected in inspector
        playerData = (PlayerData)target;

        foreach (KeyValuePair<AmmoType, Ammo> a in playerData.ammo)
        {
            playerData.ammo[a.Key].amount = EditorGUILayout.FloatField($"ammo.{a.Key}", a.Value.amount);
        }
        */
    }
}
