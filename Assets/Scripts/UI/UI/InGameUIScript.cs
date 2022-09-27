using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The main in-game UI script (or the hub)
/// </summary>
public class InGameUIScript : MonoBehaviour
{
    // TODO: (DUPLICATE) Applies to ALL UI/HUD elements. Programmatically assign render cameras for each UI canvases!!

    // References of the UI's sub-scripts
    [SerializeField]
    internal InGamePauseUIScript pauseUIScript;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Main InGameUIScript starting");
    }
}
