using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The game player manager script
/// </summary>
public class GamePlayerManager : MonoBehaviour
{
    // Reference to the game manager script
    [SerializeField]
    internal GameManager gameManager;


    [Header("All Player Prefabs")]
    [SerializeField] private PlayerScript playerPrefab; // Player prefab to spawn
    [SerializeField] private PlayerScript activePlayer; // Stored active player
    public PlayerScript PlayerPrefab { get => playerPrefab; set => playerPrefab = value; }
    internal PlayerScript ActivePlayer
    {
        get => activePlayer;
        set
        {
            activePlayer = value;

            activePlayer.inventoryScript.UpdateInventory();
            activePlayer.inventoryScript.SwitchEquipment(0);
            gameManager.InGameUI.HUDScript.hudAmmoScript.AssignWeaponScript();
            activePlayer.healthScript.OnHealthReachedZero?.RemoveListener(delegate { gameManager.gameMission.MissionEnd(MissionEndEvent.MISSION_FAILED); });
            activePlayer.healthScript.OnHealthReachedZero?.AddListener(delegate { gameManager.gameMission.MissionEnd(MissionEndEvent.MISSION_FAILED); });
        }
    }

    // Find a player object in hirearchy
    public void FindPlayerInScene()
    {
        var activePlayer = FindObjectOfType<PlayerScript>();

        if (activePlayer)
        {
            ActivePlayer = activePlayer;
        }
    }

    // Spawn player
    public void TrySpawnPlayer()
    {
        TrySpawnPlayer(PlayerPrefab.transform);
    }
    public void TrySpawnPlayer(Transform spawnPoint)
    {
        if (!ActivePlayer)
        {
            // Spawn player
            PlayerScript playerToSpawn = Instantiate(PlayerPrefab, spawnPoint.position, Quaternion.identity);

            // Get weapons/equipments
            WeaponScript weaponMelee = gameManager.gameWeapon.GetWeapon(gameManager.LoadedGameData.equippedMeleeWeapon);
            WeaponScript weaponRanged1 = gameManager.gameWeapon.GetWeapon(gameManager.LoadedGameData.equippedRangedWeapon1);
            WeaponScript weaponRanged2 = gameManager.gameWeapon.GetWeapon(gameManager.LoadedGameData.equippedRangedWeapon2);

            // Instantiate the weapons/equipments in playerToSapwn inventory
            Instantiate(weaponMelee, playerToSpawn.inventoryScript.inventoryHolder.transform, false);
            if (weaponRanged1)
                Instantiate(weaponRanged1, playerToSpawn.inventoryScript.inventoryHolder.transform, false);
            if (weaponRanged2)
                Instantiate(weaponRanged2, playerToSpawn.inventoryScript.inventoryHolder.transform, false);

            // Set active player
            ActivePlayer = playerToSpawn;

            gameManager.InGameCameras.AssignCameraTargetGroup(true);
        }
        else
        {
            ActivePlayer.gameObject.SetActive(true);
            ActivePlayer.transform.position = spawnPoint.position;
            /*
            foreach (var behaviour in gameManager.gamePlayer.ActivePlayer.GetComponents<Behaviour>())
            {
                behaviour.enabled = true;
            }
            */
            gameManager.InGameCameras.AssignCameraTargetGroup(true);
        }
    }
}
