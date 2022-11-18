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


    // Start is called just before any of the Update methods is called the first time
    private void Start()
    {
        // Find any preexisting players first
        FindPlayer();
        // TODO: Spawn player using other method (such as after generating the map)
        // Attempt to spawn Player
        SpawnPlayer();
    }


    // Find a player object in hirearchy
    private void FindPlayer()
    {
        var activePlayer = FindObjectOfType<PlayerScript>();

        if (activePlayer)
        {
            ActivePlayer = activePlayer;
        }
    }

    // Spawn player
    public void SpawnPlayer()
    {
        // TODO: Spawn point for player
        SpawnPlayer(PlayerPrefab.transform);
    }
    public void SpawnPlayer(Transform spawnPoint)
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
