using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Class for ammo
/// </summary>
[Serializable]
public class Ammo
{
    public AmmoType ammoType;
    public float amount;

    public Ammo(AmmoType ammoType, int amount)
    {
        this.ammoType = ammoType;
        this.amount = amount;
    }
}
