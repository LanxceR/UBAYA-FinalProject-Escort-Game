using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Spawnable : MonoBehaviour
{
    public GameObject prefab;
    [Range(1, 100)] public int spawnWeight;
}
