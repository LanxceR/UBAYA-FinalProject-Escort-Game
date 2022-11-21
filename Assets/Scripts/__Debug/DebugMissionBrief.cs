using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugMissionBrief : MonoBehaviour
{
    [SerializeField]
    private DebugMissionDetail[] missionDisplays = new DebugMissionDetail[3];

    // Start is called just before any of the Update methods is called the first time
    private void Start()
    {
        OnGenMissions();
    }

    public void OnGenMissions()
    {
        GameManager.Instance.gameMission.GenerateMissions(GameManager.Instance.LoadedGameData.daysPassed);
        UpdateDisplayMissions();
    }

    private void UpdateDisplayMissions()
    {
        MissionData[] missions = GameManager.Instance.MissionDatas;

        for (int i = 0; i < missions.Length; i++)
        {
            if (!missionDisplays[i]) continue;

            #region Mission details
            // Display scene target
            missionDisplays[i].scene.text = missions[i].escortScene.ToString();

            // Display escortee
            missionDisplays[i].vehicle.ClearOptions();
            List<string> vehicles = new List<string>();
            if (!missions[i].vehicle)
            {
                foreach (EscorteeID e in GameManager.Instance.LoadedGameData.ownedVehicles)
                {
                    vehicles.Add(e.ToString());
                }
            }
            else
                vehicles.Add(missions[i].vehicle.id.ToString());
            missionDisplays[i].vehicle.ClearOptions();
            missionDisplays[i].vehicle.AddOptions(vehicles);

            // Has Wpn Toggle
            missionDisplays[i].hasWpnToggle.isOn = missions[i].escorteeHasWeapon;

            // Various Details
            missionDisplays[i].zCount.text = missions[i].zombieCount.ToString();
            missionDisplays[i].baseReward.text = "$ " + missions[i].baseReward.ToString();

            // Display enemy types
            string enemiesString = "";
            foreach (Spawnable e in missions[i].enemies)
            {
                enemiesString += $"{e.prefab.name}";
            }
            missionDisplays[i].enemies.text = enemiesString;
            #endregion


            #region Equipments
            PlayerData pData = GameManager.Instance.LoadedGameData;
            List<string> weapons = new List<string>();
            missionDisplays[i].melee.ClearOptions();
            missionDisplays[i].ranged1.ClearOptions();
            missionDisplays[i].ranged2.ClearOptions();
            foreach (WeaponID w in GameManager.Instance.LoadedGameData.ownedWeapons)
            {
                WeaponScript wpn = GameManager.Instance.gameWeapon.GetWeapon(w);
                wpn.AssignComponents();

                // Display owned melee weapons
                weapons.Clear();
                if (wpn.weaponAttackScript is WeaponMeleeAttackScript)
                {
                    weapons.Add(w.ToString());
                }
                missionDisplays[i].melee.AddOptions(weapons);

                // Display owned ranged weapons
                weapons.Clear();
                if (wpn.weaponAttackScript is WeaponRangedAttackScript)
                {
                    weapons.Add(w.ToString());
                }
                missionDisplays[i].ranged1.AddOptions(weapons);
                missionDisplays[i].ranged2.AddOptions(weapons);
            }
            #endregion
        }
    }
}
