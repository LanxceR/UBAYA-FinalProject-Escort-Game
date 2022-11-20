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

    /// <summary>
    /// <b>DEPRECATED - Don't use this flag!</b>
    /// </summary>
    public bool isEmpty = true;
    public int index;
    public Difficulty difficulty;
    public float money;
    public int daysPassed;
    public int missionsCompleted;
    public int missionsFailed;

    public List<WeaponID> ownedWeapons;
    public List<EscorteeID> ownedVehicles;

    public WeaponID equippedMeleeWeapon;
    public WeaponID equippedRangedWeapon1;
    public WeaponID equippedRangedWeapon2;
    public EscorteeID equippedVehicle;
    

    public Dictionary<AmmoType, Ammo> ammo = new Dictionary<AmmoType, Ammo>
    {
        {AmmoType.LIGHT, new Ammo(AmmoType.LIGHT, 0) },
        {AmmoType.SHOTGUN, new Ammo(AmmoType.SHOTGUN, 0) },
        {AmmoType.HEAVY, new Ammo(AmmoType.HEAVY, 0) }
    };

    #endregion

    #region Constructors
    internal PlayerData()
    {
        isEmpty = true;
    }
    public PlayerData(int index, Difficulty difficulty, float money, int daysPassed, int missionsCompleted, int missionsFailed, float ammo_LIGHT, float ammo_SHOTGUN, float ammo_HEAVY,
                      List<WeaponID> ownedWeapons, List<EscorteeID> ownedVehicles, WeaponID equippedMeleeWeapon, WeaponID equippedRangedWeapon1, WeaponID equippedRangedWeapon2, EscorteeID equippedVehicle)
    {
        this.isEmpty = false;
        this.index = index;
        this.difficulty = difficulty;
        this.money = money;
        this.daysPassed = daysPassed;
        this.missionsCompleted = missionsCompleted;
        this.missionsFailed = missionsFailed;
        this.ammo[AmmoType.LIGHT].amount = ammo_LIGHT;
        this.ammo[AmmoType.SHOTGUN].amount = ammo_SHOTGUN;
        this.ammo[AmmoType.HEAVY].amount = ammo_HEAVY;

        this.ownedWeapons = ownedWeapons;
        this.ownedVehicles = ownedVehicles;

        this.equippedMeleeWeapon = equippedMeleeWeapon;
        this.equippedRangedWeapon1 = equippedRangedWeapon1;
        this.equippedRangedWeapon2 = equippedRangedWeapon2;
        this.equippedVehicle = equippedVehicle;
    }
    #endregion

    // TODO: Update the methods to include ammo checking
    public void Empty()
    {
        this.isEmpty = true;
        this.difficulty = Difficulty.CASUAL;
        this.money = 0;
        this.daysPassed = 0;
        this.missionsCompleted = 0;
        this.missionsFailed = 0;

        this.ownedWeapons.Clear();
        this.ownedVehicles.Clear();

        this.equippedMeleeWeapon = WeaponID.NONE;
        this.equippedRangedWeapon1 = WeaponID.NONE;
        this.equippedRangedWeapon2 = WeaponID.NONE;
        this.equippedVehicle = EscorteeID.NONE;
    }

    public bool IsEmpty()
    {
        return (money == 0) &&
            (daysPassed == 0) &&
            (missionsCompleted == 0) &&
            Utilities.IsListContentEquals(ownedWeapons, new List<WeaponID>()) &&
            Utilities.IsListContentEquals(ownedVehicles, new List<EscorteeID>()) &&
            (missionsFailed == 0);
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
            (missionsFailed == data.missionsFailed) &&
            Utilities.IsListContentEquals(ownedWeapons, data.ownedWeapons) &&
            Utilities.IsListContentEquals(ownedVehicles, data.ownedVehicles) &&
            (equippedMeleeWeapon == data.equippedMeleeWeapon) &&
            (equippedRangedWeapon1 == data.equippedRangedWeapon1) &&
            (equippedRangedWeapon2 == data.equippedRangedWeapon2) &&
            (equippedVehicle == data.equippedVehicle);
    }

    public bool Equals(PlayerData data)
    {
        if ((object)data == null)
            return false;

        return (isEmpty == data.isEmpty) &&
            (index == data.index) &&
            ((int)difficulty == (int)data.difficulty) &&
            (money == data.money) &&
            (daysPassed == data.daysPassed) &&
            (missionsCompleted == data.missionsCompleted) &&
            (missionsFailed == data.missionsFailed) &&
            Utilities.IsListContentEquals(ownedWeapons, data.ownedWeapons) &&
            Utilities.IsListContentEquals(ownedVehicles, data.ownedVehicles) &&
            ((int)equippedMeleeWeapon == (int)data.equippedMeleeWeapon) &&
            ((int)equippedRangedWeapon1 == (int)data.equippedRangedWeapon1) &&
            ((int)equippedRangedWeapon2 == (int)data.equippedRangedWeapon2) &&
            ((int)equippedVehicle == (int)data.equippedVehicle);
    }

    public override int GetHashCode()
    {
        return isEmpty.GetHashCode() ^
            index.GetHashCode() ^
            difficulty.GetHashCode() ^
            money.GetHashCode() ^
            daysPassed.GetHashCode() ^
            missionsCompleted.GetHashCode() ^
            missionsFailed.GetHashCode() ^
            ownedWeapons.GetHashCode() ^
            ownedVehicles.GetHashCode() ^
            equippedMeleeWeapon.GetHashCode() ^
            equippedRangedWeapon1.GetHashCode() ^
            equippedRangedWeapon2.GetHashCode() ^
            equippedVehicle.GetHashCode();
    }
}
