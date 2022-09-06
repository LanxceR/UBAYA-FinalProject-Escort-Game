using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Object Follow another object behaviour
/// </summary>
public class FollowObject : MonoBehaviour
{
    // TODO: Programmatically assign this object to follow for various overhead HUD elements
    // Components
    [Header("Components")]
    [SerializeField]
    private GameObject objectToFollow;
    
    // Settings
    [Header("Settings")]
    [SerializeField] private Vector2 offset;
    [SerializeField] private bool disableZFollow;

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = objectToFollow.transform.position;

        transform.position = new Vector3(newPos.x, 
                                         newPos.y,
                                         disableZFollow ? 0f : newPos.z);
    }
}
