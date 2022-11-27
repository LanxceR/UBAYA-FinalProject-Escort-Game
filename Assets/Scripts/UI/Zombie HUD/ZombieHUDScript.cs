using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHUDScript : MonoBehaviour
{
    // Start is called before the first frame update
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
