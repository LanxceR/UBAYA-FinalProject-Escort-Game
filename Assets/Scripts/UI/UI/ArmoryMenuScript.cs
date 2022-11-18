using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;


[System.Serializable]
public class Weapon
{
    public string weaponName;
    public float weaponPrice;
    public Sprite weaponImage;
    public float dmgValue;
    public bool isEquipped;
    public bool isPurchased;

    [Header("Ranged-only Properties")]
    public Ammo ammo;
    public float ammoPerMag;

    [Header("Melee-only Properties")]
    public float spread;
    public float range;
}

public class ArmoryMenuScript : MonoBehaviour
{
    public Weapon[] weapons;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
