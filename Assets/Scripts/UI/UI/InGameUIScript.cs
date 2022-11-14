using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The main in-game UI script (or the hub)
/// </summary>
public class InGameUIScript : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    internal Canvas canvas;

    // References of the UI's sub-scripts
    [SerializeField]
    internal InGamePauseUIScript pauseUIScript;
    [SerializeField]
    internal InGameGameOverUIScript gameOverUIScript;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Main InGameUIScript starting");

        // Assign world space event camera
        canvas.worldCamera = GameManager.Instance.Cameras.UICamera;
    }
}
