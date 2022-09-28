using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The main HUD script (or the hub)
/// </summary>
public class HUDScript : MonoBehaviour
{
    // TODO: (DUPLICATE) Applies to ALL UI/HUD elements. Programmatically assign render cameras for each UI canvases!!

    // References of the HUD's sub-scripts
    [SerializeField]
    internal HUDAmmoScript hudAmmoScript;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Main HUDScript starting");
    }
}
