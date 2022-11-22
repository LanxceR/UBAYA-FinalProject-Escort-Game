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
    private float amount;
    public float maxAmount;
    public float amountPerStack;
    public float price;

    public float Amount { get => amount;
        set 
        {
            amount = Mathf.Clamp(value, 0, maxAmount);
        } 
    }

    public Ammo(AmmoType ammoType, float amount, float maxAmount, float amountPerStack, float price)
    {
        this.ammoType = ammoType;
        this.Amount = Math.Clamp(amount, 0, maxAmount);
        this.maxAmount = maxAmount;
        this.amountPerStack = amountPerStack;
        this.price = price;
    }
}
