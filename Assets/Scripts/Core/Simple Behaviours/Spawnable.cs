using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Spawnable
{
    public GameObject prefab;
    [Range(0, 100)] public float spawnWeight;

    public Spawnable(GameObject prefab, float spawnWeight)
    {
        this.prefab = prefab;
        this.spawnWeight = spawnWeight;
    }
}
