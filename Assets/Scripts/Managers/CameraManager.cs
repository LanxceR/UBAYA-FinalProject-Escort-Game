using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

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

    internal CinemachineTargetGroup mainCameraTargetGroup;

    // Start is called just before any of the Update methods is called the first time
    private void Start()
    {
        mainCameraTargetGroup = mainCamera.GetComponentInChildren<CinemachineTargetGroup>();
    }

    // Assign a new target group for the camera
    internal void AssignCameraTargetGroup(bool centerAroundPlayer)
    {
        PlayerScript player = GameManager.Instance.ActivePlayer;
        if (centerAroundPlayer && player)
        {
            // Set up player and aimHelper's target
            CinemachineTargetGroup.Target playerTarget;
            CinemachineTargetGroup.Target aimTarget;

            // Set the weight and radius
            // For this weight ratio 3:7 is used for player:aimTarget 
            // e.g: player <---1m---> aimTarget, then 
            // player <-0.3m-> O <--0.7m--> aimTarget
            playerTarget = new CinemachineTargetGroup.Target { target = player.transform, weight = 0.70f, radius = 0f };
            aimTarget = new CinemachineTargetGroup.Target { target = player.GetComponentInChildren<FollowMouseWithRadius>().transform, weight = 0.30f, radius = 0f };

            CinemachineTargetGroup.Target[] targets = { playerTarget, aimTarget};

            SetTargetGroup(targets);
        }
    }

    // Setting up target group
    private void SetTargetGroup(CinemachineTargetGroup.Target[] targets)
    {
        // Clear all targets in target group
        mainCameraTargetGroup.m_Targets = new CinemachineTargetGroup.Target[0];

        foreach (CinemachineTargetGroup.Target t in targets)
        {
            // Add all targets including their weight and radius
            mainCameraTargetGroup.AddMember(t.target, t.weight, t.radius);
        }
    }
}
