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

    // Component
    [Header("Mount Setting")]
    [SerializeField]
    private bool isMounted;
    [SerializeField]
    private Collider2D mountPos;
    [SerializeField]
    private Vector2 offsetPos;

    // Variable
    private FixedJoint2D anchor;

    public void PerformInteraction(GameObject actor)
    {
        Debug.Log($"{actor.name} has interacted with {gameObject.name}");

        MountAction(actor);
    }

    private void MountAction(GameObject actor)
    {
        if (!isMounted) // If not mounted, then mount
        {
            // Add a FixedJoint2D to the actor
            anchor = actor.AddComponent<FixedJoint2D>();
            // Connect the actor to this rigidbody
            anchor.connectedBody = GetComponent<Rigidbody2D>();
            // Disable anchor auto calculation
            anchor.autoConfigureConnectedAnchor = false;
            // Adjust anchor position (mounting position)
            Vector2 pos = mountPos.offset + offsetPos;
            anchor.connectedAnchor = pos;
            // Set isMounted to true
            isMounted = true;

            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Convoy/Mount");
        }
        else // Otherwise, dismount
        {
            // Destroy FixedJoint2D component on the actor
            Destroy(anchor);
            // Set isMounted to false
            isMounted = false;

            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Convoy/Dismount");
        }
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
