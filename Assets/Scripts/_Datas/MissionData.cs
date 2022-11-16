using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Class for serializing mission data/details (use this to pass to a level manager)
/// </summary>
public enum HazardRating { NORMAL, INFESTED, OVERRUN}
[Serializable]
public class MissionData
{
    #region Data fields

    public GameObject vehicle;
    public bool escorteeHasWeapon;
    public int zombieCount;
    public float baseReward;

    #endregion

    #region Constructors
    public MissionData(GameObject vehicle, bool escorteeHasWeapon, int zombieCount, float baseReward)
    {
        this.vehicle = vehicle;
        this.escorteeHasWeapon = escorteeHasWeapon;
        this.zombieCount = zombieCount;
        this.baseReward = baseReward;
    }
    #endregion

    public override bool Equals(System.Object obj)
    {
        if (obj == null)
            return false;

        MissionData data = obj as MissionData;
        if ((System.Object)data == null)
            return false;

        return (vehicle == data.vehicle) &&
            (escorteeHasWeapon == data.escorteeHasWeapon) &&
            (zombieCount == data.zombieCount) &&
            (baseReward == data.baseReward);
    }

    public bool Equals(MissionData data)
    {
        if ((object)data == null)
            return false;

        return (vehicle == data.vehicle) &&
            (escorteeHasWeapon == data.escorteeHasWeapon) &&
            (zombieCount == data.zombieCount) &&
            (baseReward == data.baseReward);
    }
}
