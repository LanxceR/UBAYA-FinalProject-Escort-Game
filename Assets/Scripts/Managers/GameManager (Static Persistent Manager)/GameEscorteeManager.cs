using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The game escortee manager script
/// </summary>
public class GameEscorteeManager : MonoBehaviour
{
    // Reference to the game manager script
    [SerializeField]
    internal GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        // Find any preexisting players first
        FindEscortee();
        // TODO: Spawn player using other method (such as after generating the map)
        // Attempt to spawn Player
        SpawnEscortee();
    }

    // Find a player object in hirearchy
    private void FindEscortee()
    {
        var activeEscortee = FindObjectOfType<EscorteeScript>();

        if (activeEscortee)
        {
            gameManager.ActiveEscortee = activeEscortee;
        }
    }

    // Spawn player
    public void SpawnEscortee()
    {
        // TODO: Spawn point for escortee & Assign loaded mission escortee
        //SpawnEscortee(gameManager.LoadedMissionData.vehicle.transform);
    }
    public void SpawnEscortee(Transform spawnPoint)
    {
        if (!gameManager.ActiveEscortee)
        {
            gameManager.ActiveEscortee = Instantiate(gameManager.LoadedMissionData.vehicle, spawnPoint.position, Quaternion.identity);
        }
        else
        {
            gameManager.ActiveEscortee.gameObject.SetActive(true);
            gameManager.ActiveEscortee.transform.position = spawnPoint.position;
            /*
            foreach (var behaviour in gameManager.ActiveEscortee.GetComponents<Behaviour>())
            {
                behaviour.enabled = true;
            }
            */
        }
    }
}
