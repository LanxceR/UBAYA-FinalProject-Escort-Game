using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles all object pooling for object pool managers
/// </summary>
public class ObjectPooler : MonoBehaviour
{
    // Singleton instance
    private static ObjectPooler instance;

    // Settings
    [Header("Main Setting")]
    [SerializeField] private Transform parent;
    [SerializeField] private int size; // Amount to spawn
    [SerializeField] private PoolObject[] prefabs; // Array of prefabs to be made as pooled objects

    // Prefabs
    [Header("Stored Objects")]
    [SerializeField] private List<PoolObject> poolObjects;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called just before any of the Update methods is called the first time
    private void Start()
    {
        Debug.Log("ObjectPooler script starting");
        // Instantiate bunch of pool objects at start
        InstantiateObjects();
    }

    // Get this singleton instance
    internal static ObjectPooler GetInstance()
    {
        return instance;
    }

    // Instantiate pool objects
    private void InstantiateObjects()
    {
        poolObjects = new List<PoolObject>();
        for (int i = 0; i < size; i++)
        {
            // Spawn all objects in Prefabs[] array * Size
            foreach (PoolObject obj in prefabs)
            {
                poolObjects.Add(Instantiate(obj.gameObject, parent).GetComponent<PoolObject>());
            }
        }
    }

    // Request an inactive pooled object
    internal PoolObject RequestObject(PoolObjectType type)
    {
        foreach (PoolObject obj in poolObjects)
        {
            // Look for an inactive object in the pool array, then fetch it
            if (obj.ObjectType == type && !obj.IsActive())
            {
                return obj;
            }
        }
        // Otherwise fetch nothing
        return null;
    }

    // Deactivate all pooled objects
    internal void DeactivateAllPoolObjects()
    {
        foreach (PoolObject obj in poolObjects)
        {
            obj.Deactivate();
        }
    }
}
