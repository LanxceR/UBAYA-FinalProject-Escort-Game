using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DebugMissionDetail : MonoBehaviour
{
    [SerializeField]
    internal int index;

    [Header("UI Refs")]
    [SerializeField]
    internal TMP_Dropdown vehicle;
    [SerializeField]
    internal Toggle hasWpnToggle;

    [SerializeField]
    internal TextMeshProUGUI zCount;
    [SerializeField]
    internal TextMeshProUGUI baseReward;
    [SerializeField]
    internal TextMeshProUGUI enemies;

    [SerializeField]
    internal TMP_Dropdown melee;
    [SerializeField]
    internal TMP_Dropdown ranged1;
    [SerializeField]
    internal TMP_Dropdown ranged2;

    public void OnPlay()
    {
        // Get escortee/vehicle
        Enum.TryParse(vehicle.captionText.text, out EscorteeID eID);
        EscorteeScript escortee = GameManager.Instance.gameEscortee.GetEscortee(eID);

        // Get weapons
        Enum.TryParse(melee.captionText.text, out WeaponID mID);
        WeaponScript meleeWeapon = GameManager.Instance.gameMission.GetWeapon(mID);

        Enum.TryParse(ranged1.captionText.text, out WeaponID rID1);
        WeaponScript rangedWeapon1 = GameManager.Instance.gameMission.GetWeapon(rID1);

        Enum.TryParse(ranged2.captionText.text, out WeaponID rID2);
        WeaponScript rangedWeapon2 = GameManager.Instance.gameMission.GetWeapon(rID2);

        GameManager.Instance.gameMission.LoadMission(index, escortee, meleeWeapon, rangedWeapon1, rangedWeapon2);
    }
}
