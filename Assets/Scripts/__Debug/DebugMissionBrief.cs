using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugMissionBrief : MonoBehaviour
{
    [SerializeField]
    private DebugMissionDetail[] missionDisplays = new DebugMissionDetail[3];

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

            foreach (WeaponID w in GameManager.Instance.LoadedGameData.ownedWeapons)
            {
                
            }
            #endregion
        }
    }
}
