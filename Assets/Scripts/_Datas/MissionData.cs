using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Class for serializing mission data/details (use this to pass to a level manager)
/// </summary>
public enum HazardRating { NORMAL, INFESTED, OVERRUN, APOCALYPSE}
[Serializable]
public class MissionData
{
    #region Data fields

    public SceneName escortScene = SceneName.TITLE_SCREEN;

    public bool isFinalMission;
    public EscorteeScript vehicle;
    public bool escorteeHasWeapon;
    public int zombieCount;
    public float baseReward;

    // DEPRECATED
    public WeaponScript meleeWeapon;
    public WeaponScript rangedWeapon1;
    public WeaponScript rangedWeapon2;
    //

    public List<Spawnable> enemies;

    #endregion

    #region Constructors
    public MissionData(SceneName escortScene, EscorteeScript vehicle, bool escorteeHasWeapon, int zombieCount, float baseReward, WeaponScript meleeWeapon, WeaponScript rangedWeapon1, WeaponScript rangedWeapon2, List<Spawnable> enemies, bool isFinalMission = false)
    {
        this.isFinalMission = isFinalMission;

        this.escortScene = escortScene;

        this.vehicle = vehicle;
        this.escorteeHasWeapon = escorteeHasWeapon;
        this.zombieCount = zombieCount;
        this.baseReward = baseReward;

        this.meleeWeapon = meleeWeapon;
        this.rangedWeapon1 = rangedWeapon1;
        this.rangedWeapon2 = rangedWeapon2;

        this.enemies = enemies;
    }
    public MissionData(SceneName escortScene, EscorteeScript vehicle, bool escorteeHasWeapon, int zombieCount, float baseReward, List<Spawnable> enemies, bool isFinalMission = false)
    {
        this.isFinalMission = isFinalMission;

        this.escortScene = escortScene;

        this.vehicle = vehicle;
        this.escorteeHasWeapon = escorteeHasWeapon;
        this.zombieCount = zombieCount;
        this.baseReward = baseReward;

        this.enemies = enemies;
        this.escortScene = escortScene;
    }
    public MissionData(SceneName escortScene, bool escorteeHasWeapon, int zombieCount, float baseReward, List<Spawnable> enemies, bool isFinalMission = false)
    {
        this.isFinalMission = isFinalMission;

        this.escortScene = escortScene;

        this.escorteeHasWeapon = escorteeHasWeapon;
        this.zombieCount = zombieCount;
        this.baseReward = baseReward;

        this.enemies = enemies;
    }
    #endregion

    public void Empty()
    {
        this.escortScene = SceneName.TITLE_SCREEN;

        this.vehicle = null;
        this.escorteeHasWeapon = false;
        this.zombieCount = 0;
        this.baseReward = 0;

        this.meleeWeapon = null;
        this.rangedWeapon1 = null;
        this.rangedWeapon2 = null;

        this.enemies.Clear();
    }

    public bool IsEmpty()
    {
        return (escortScene == SceneName.TITLE_SCREEN) &&
            (vehicle == null) &&
            (escorteeHasWeapon == false) &&
            (zombieCount == 0) &&
            (baseReward == 0) &&
            (enemies == null || Utilities.IsListContentEquals(enemies, new List<Spawnable>()));
    }

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
