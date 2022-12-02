using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The player interaction script (handles all player interact actions)
/// </summary>
public class PlayerInteractScript : MonoBehaviour
{
    // Reference to the main player script
    [SerializeField]
    private PlayerScript playerScript;

    [SerializeField]
    private LayerMask interactableLayer = 1 << 12; // Interactable layer

    [SerializeField]
    private IInteractable interactTarget;

    // Coroutine
    private Coroutine interactionCoroutine;

    // Update is called once per frame
    void Update()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.2f, interactableLayer);

        // If there's an interactable object detected
        if (hits.Length > 0)
        {
            // Compare all hit target tags with targetTags list
            foreach (Collider2D hit in hits)
            {
                IInteractable interacted = Utilities.FindParentOfType<IInteractable>(hit.transform, out _);

                // If there are no targets OR the new target is closer
                if (interactTarget == null || (hit.transform.position - transform.position).sqrMagnitude < (interactTarget.GetTransform().position - transform.position).sqrMagnitude)
                {
                    // Set new target
                    interactTarget = interacted;
                }
            }
        }
        else // Otherwise, set target to null
        {
            interactTarget = null;
        }

        if (playerScript.playerInputScript.Input_Interact == 1 && interactTarget != null)
        {
            if (interactionCoroutine == null)
                interactionCoroutine = StartCoroutine(Interaction());
        }
    }

    private IEnumerator Interaction()
    {
        interactTarget.PerformInteraction(gameObject);

        while (playerScript.playerInputScript.Input_Interact == 1)
        {
            // While attack button is still performed, wait until the next frame
            yield return null;
        }

        // After interact button is released/canceled, set Interaction coroutine to null
        interactionCoroutine = null;
    }

    public bool CheckIfInteractableExists()
    {
        if(interactTarget != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
