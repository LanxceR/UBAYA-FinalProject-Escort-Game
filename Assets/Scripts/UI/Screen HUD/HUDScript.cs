using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The main HUD script (or the hub)
/// </summary>
public class HUDScript : MonoBehaviour
{
    // TODO: Implement escortee related HUD (Escortee Speed)
    // TODO: Implement progress/journey bar

    [Header("Components")]
    [SerializeField]
    internal Canvas canvas;

    // References of the HUD's sub-scripts
    [SerializeField]
    internal HUDAmmoScript hudAmmoScript;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Main HUDScript starting");

        // Assign world space event camera
        canvas.worldCamera = GameManager.Instance.InGameCameras.HUDCamera;
    }
}
