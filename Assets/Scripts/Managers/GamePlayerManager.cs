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
            gameManager.ActivePlayer = activePlayer;
        }
    }

    // Spawn player
    public void SpawnPlayer()
    {
        // TODO: Spawn point for player
        SpawnPlayer(gameManager.PlayerPrefab.transform);
    }
    public void SpawnPlayer(Transform spawnPoint)
    {
        if (!gameManager.ActivePlayer)
        {
            gameManager.ActivePlayer = Instantiate(gameManager.PlayerPrefab, spawnPoint.position, Quaternion.identity);
            gameManager.Cameras.AssignCameraTargetGroup(true);
        }
        else
        {
            gameManager.ActivePlayer.gameObject.SetActive(true);
            gameManager.ActivePlayer.transform.position = spawnPoint.position;
            /*
            foreach (var behaviour in gameManager.ActivePlayer.GetComponents<Behaviour>())
            {
                behaviour.enabled = true;
            }
            */
            gameManager.Cameras.AssignCameraTargetGroup(true);
        }
    }
}
