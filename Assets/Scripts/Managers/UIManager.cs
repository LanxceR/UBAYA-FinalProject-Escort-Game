using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI/HUD Prefabs")]
    [SerializeField] private HUDScript hudScript;
    [SerializeField] private InGameUIScript uiScript;

    public HUDScript HUDScript { get => hudScript; private set => hudScript = value; }
    public InGameUIScript UIScript { get => uiScript; private set => uiScript = value; }
}