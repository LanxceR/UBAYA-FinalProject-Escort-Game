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
    /// <summary>
    /// Get an escortee instance of a type
    /// </summary>
    /// <param name="escorteeType">The escortee type</param>
    /// <returns></returns>
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
    public void UpdateAllEscorteeFlags()
    {

        foreach (EscorteeScript e in EscorteePrefabs)
        {
            // First set ALL escortees flags to false
            e.isOwned = false;
            e.isEquipped = false;
        }

        // Set all isOwned flags
        foreach (EscorteeID w in gameManager.LoadedGameData.ownedVehicles)
        {
            // Then set owned isOwned flags to true
            GetEscortee(w).isOwned = true;
        }

        // Set all isEquipped flags
        EscorteeID equippedEscortee;
        equippedEscortee = gameManager.LoadedGameData.equippedVehicle;
        GetEscortee(equippedEscortee).isEquipped = true;
    }
    public void SetEscorteeOwnedFlag(EscorteeID escorteeType, bool isOwned)
    {
        GetEscortee(escorteeType).isOwned = isOwned;
    }
    public void SetEscorteeEquippedFlag(EscorteeID escorteeType, bool isEquipped)
    {
        GetEscortee(escorteeType).isEquipped = isEquipped;
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
