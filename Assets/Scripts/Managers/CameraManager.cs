using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The game camera manager script
/// </summary>
public class CameraManager : MonoBehaviour
{
    [Header("Cameras Prefabs")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera hudCamera;
    [SerializeField] private Camera uiCamera;

    public Camera MainCamera { get => mainCamera; private set => mainCamera = value; }
    public Camera HUDCamera { get => hudCamera; private set => hudCamera = value; }
    public Camera UICamera { get => uiCamera; private set => uiCamera = value; }
}
