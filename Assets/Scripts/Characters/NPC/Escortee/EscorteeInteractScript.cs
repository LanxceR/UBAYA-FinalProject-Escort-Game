using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The escortee interact script (detects & store all escortee interactions)
/// </summary>
public class EscorteeInteractScript : MonoBehaviour, IInteractable
{
    // Reference to the main player script
    [SerializeField]
    private EscorteeScript escorteeScript;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PerformInteraction(GameObject actor)
    {
        Debug.Log($"{actor.name} has interacted with {gameObject.name}");
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
