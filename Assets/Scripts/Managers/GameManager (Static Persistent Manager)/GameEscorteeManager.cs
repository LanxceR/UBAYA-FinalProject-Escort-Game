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


    [Header("All Escortee Prefabs")]
    [SerializeField] private EscorteeScript[] escorteePrefabs; // Escortee prefab to spawn
    [SerializeField] private EscorteeScript activeEscortee; // Stored active escortee
    public EscorteeScript[] EscorteePrefabs { get => escorteePrefabs; set => escorteePrefabs = value; }
    internal EscorteeScript ActiveEscortee { get => activeEscortee; set => activeEscortee = value; }

    #region Prefab Utilities
    // TODO: (DUPLICATE) Maybe put these methods in their corresponding scripts and load using Resources.Load
    public EscorteeScript GetEscortee(EscorteeID escorteeType)
    {
        // Find an enemy of a certain type
        foreach (EscorteeScript e in EscorteePrefabs)
        {
            if (e.id == escorteeType) return e;
        }

        // If nothing is found, return null
        return null;
    } 
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // Find any preexisting players first
        FindEscorteeInScene();
        // TODO: Spawn player using other method (such as after generating the map)
        // Attempt to spawn Player
        //SpawnEscortee();
    }

    // Find a player object in hirearchy
    private void FindEscorteeInScene()
    {
        var activeEscortee = FindObjectOfType<EscorteeScript>();

        if (activeEscortee)
        {
            ActiveEscortee = activeEscortee;
        }
    }

    // Spawn player
    public void SpawnEscortee()
    {
        // TODO: Spawn point for escortee & Assign loaded mission escortee
        SpawnEscortee(gameManager.LoadedMissionData.vehicle.transform);
    }
    public void SpawnEscortee(Transform spawnPoint)
    {
        if (!ActiveEscortee)
        {
            ActiveEscortee = Instantiate(gameManager.LoadedMissionData.vehicle, spawnPoint.position, Quaternion.identity);
        }
        else
        {
            ActiveEscortee.gameObject.SetActive(true);
            ActiveEscortee.transform.position = spawnPoint.position;
            /*
            foreach (var behaviour in ActiveEscortee.GetComponents<Behaviour>())
            {
                behaviour.enabled = true;
            }
            */
        }
    }
}
