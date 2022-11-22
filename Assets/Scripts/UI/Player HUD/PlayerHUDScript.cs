using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The main player attached HUD script (or the hub)
/// </summary>
public class PlayerHUDScript : MonoBehaviour
{
    // TODO: Implement a floating "key to press" UI for interactions ([F] Interact)

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

        // Assign world space render camera
        canvas.worldCamera = GameManager.Instance.InGameCameras.MainCamera;
    }
}
