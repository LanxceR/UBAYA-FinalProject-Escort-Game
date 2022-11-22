using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ConvoyList
{
    public string convoyName;
    public float convoyPrice;
    public Sprite convoyImage;
    public float healthValue;
    public float maxSpeedValue;
    public bool isEquipped;
    public bool isPurchased;
}

public class ConvoyScript : MonoBehaviour
{
    public ConvoyList[] convoys;
}
