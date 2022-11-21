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
    public float maxAmount;
    public float amountPerStack;
    public float price;

    public Ammo(AmmoType ammoType, int amount, float maxAmount, float amountPerStack, float price)
    {
        this.ammoType = ammoType;
        this.amount = Math.Clamp(amount, 0, maxAmount);
        this.maxAmount = maxAmount;
        this.amountPerStack = amountPerStack;
        this.price = price;
    }
}
