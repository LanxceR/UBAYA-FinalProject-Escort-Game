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
    // TODO: Implement zombie types to spawn checks
    public EscorteeScript vehicle;
    public bool escorteeHasWeapon;
    public int zombieCount;
    public float baseReward;

    public List<Spawnable> enemies;

    #endregion

    #region Constructors
    public MissionData(EscorteeScript vehicle, bool escorteeHasWeapon, int zombieCount, float baseReward, List<Spawnable> enemies)
    {
        this.vehicle = vehicle;
        this.escorteeHasWeapon = escorteeHasWeapon;
        this.zombieCount = zombieCount;
        this.baseReward = baseReward;

        this.enemies = enemies;
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
            (baseReward == data.baseReward) &&
            (enemies == data.enemies);
    }

    public bool Equals(MissionData data)
    {
        if ((object)data == null)
            return false;

        return (vehicle == data.vehicle) &&
            (escorteeHasWeapon == data.escorteeHasWeapon) &&
            (zombieCount == data.zombieCount) &&
            (baseReward == data.baseReward) &&
            (enemies == data.enemies);
    }
}
