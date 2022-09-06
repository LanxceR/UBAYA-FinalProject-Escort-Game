using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The main HUD script (or the hub)
/// </summary>
public class PlayerHUDScript : MonoBehaviour
{
    // References of the HUD's sub-scripts
    [SerializeField]
    internal PlayerHUDReloadIndicatorScript pHUDReloadScript;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Main PlayerHUDScript starting");
    }
}
