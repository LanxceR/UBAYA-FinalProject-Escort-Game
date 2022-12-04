using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The item drop script
/// </summary>
public class ItemDropScript : MonoBehaviour
{
    // Variables
    [SerializeField] private List<Spawnable> items;

    // Drop an item
    internal void SpawnItem(Vector2 spawnPos)
    {
        // Randomize item to spawn
        GameObject itemToSpawn = GetWeightedRandomizedItem();

        // If there is an item to spawn
        if (itemToSpawn)
            Instantiate(itemToSpawn, spawnPos, Quaternion.identity);
    }

    private GameObject GetWeightedRandomizedItem()
    {
        // Add total weight
        float totalWeight = 0;
        foreach (Spawnable s in items)
        {
            totalWeight += s.spawnWeight;
        }

        // Generate a random number from 1 - totalWeight
        float rand = Random.Range(1f, totalWeight);

        // Weighted randomize pick an enemy to spawn
        float pos = 0;
        for (int j = 0; j < items.Count; j++)
        {
            if (rand <= items[j].spawnWeight + pos)
            {
                // Return selected item
                return items[j].prefab;
            }
            pos += items[j].spawnWeight;
        }

        // Nothing to choose from
        return null;
    }
}
