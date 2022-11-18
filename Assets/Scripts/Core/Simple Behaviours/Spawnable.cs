using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Spawnable
{
    public GameObject prefab;
    [Range(1, 100)] public int spawnWeight;

    public Spawnable(GameObject prefab, int spawnWeight)
    {
        this.prefab = prefab;
        this.spawnWeight = spawnWeight;
    }
}
