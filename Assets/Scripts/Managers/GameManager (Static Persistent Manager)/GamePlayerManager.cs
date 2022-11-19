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
            gameManager.InGameUI.HUDScript.hudAmmoScript.AssignWeaponScript();
        }
    }

    // Find a player object in hirearchy
    public void FindPlayer()
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
        // TODO: Spawn point for player
        TrySpawnPlayer(PlayerPrefab.transform);
    }
    public void TrySpawnPlayer(Transform spawnPoint)
    {
        if (!ActivePlayer)
        {
            ActivePlayer = Instantiate(PlayerPrefab, spawnPoint.position, Quaternion.identity);
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
