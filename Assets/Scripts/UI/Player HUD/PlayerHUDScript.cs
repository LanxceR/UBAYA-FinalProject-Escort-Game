using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The main player attached HUD script (or the hub)
/// </summary>
public class PlayerHUDScript : MonoBehaviour
{
    // References of the HUD's sub-scripts
    [Header("Sub-scripts")]
    [SerializeField]
    internal PlayerHUDReloadIndicatorScript pHUDReloadScript;

    [Header("Components")]
    [SerializeField]
    internal Canvas canvas;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Main PlayerHUDScript starting");

        // Assign world space event camera
        canvas.worldCamera = GameManager.Instance.Cameras.HUDCamera;
    }
}
