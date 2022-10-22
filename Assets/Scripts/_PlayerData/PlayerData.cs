using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Class for serializing savefiles (use this class as an instance to serialize player datas for storing)
/// </summary>
public enum Difficulty { CASUAL, HARDCORE}
[Serializable]
public class PlayerData
{
    #region Data fields

    public bool isEmpty = false;
    public int index;
    public Difficulty difficulty;
    public float money;
    public int daysPassed;
    public int missionsCompleted;
    public int missionsFailed;


    #endregion

    #region Constructors
    internal PlayerData()
    {
        isEmpty = true;
    }
    public PlayerData(int index, Difficulty difficulty, float money, int daysPassed, int missionsCompleted, int missionsFailed)
    {
        this.isEmpty = false;
        this.index = index;
        this.difficulty = difficulty;
        this.money = money;
        this.daysPassed = daysPassed;
        this.missionsCompleted = missionsCompleted;
        this.missionsFailed = missionsFailed;
    }
    #endregion

    public void Empty()
    {
        this.isEmpty = true;
        this.difficulty = 0;
        this.money = 0;
        this.daysPassed = 0;
        this.missionsCompleted = 0;
        this.missionsFailed = 0;
    }

    public override bool Equals(System.Object obj)
    {
        if (obj == null)
            return false;

        PlayerData data = obj as PlayerData;
        if ((System.Object)data == null)
            return false;

        return (isEmpty == data.isEmpty) &&
            (index == data.index) &&
            (difficulty == data.difficulty) &&
            (money == data.money) &&
            (daysPassed == data.daysPassed) &&
            (missionsCompleted == data.missionsCompleted) &&
            (missionsFailed == data.missionsFailed);
    }

    public bool Equals(PlayerData data)
    {
        if ((object)data == null)
            return false;

        return (isEmpty == data.isEmpty) &&
            (index == data.index) &&
            (difficulty == data.difficulty) &&
            (money == data.money) &&
            (daysPassed == data.daysPassed) &&
            (missionsCompleted == data.missionsCompleted) &&
            (missionsFailed == data.missionsFailed);
    }

    public override int GetHashCode()
    {
        return isEmpty.GetHashCode() ^
            index.GetHashCode() ^
            difficulty.GetHashCode() ^
            money.GetHashCode() ^
            daysPassed.GetHashCode() ^
            missionsCompleted.GetHashCode() ^
            missionsFailed.GetHashCode();
    }
}
