using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Behaviours for finish trigger objects
/// </summary>
[RequireComponent(typeof(CollisionScript))]
public class FinishTrigger : MonoBehaviour
{
    [Header("Main Setting")]
    [SerializeField] private CollisionScript collisionScript;

    // Variables
    [Header("Variables")]
    [SerializeField] private bool escorteeHasPassed;
    [SerializeField] private bool playerHasPassed;

    // Components
    LevelManager lvlManager;

    // Start is called before the first frame update
    void Start()
    {
        lvlManager = Utilities.FindParentOfType<LevelManager>(transform, out _);

        if (collisionScript)
        {
            // Add listener to collision UnityEvents
            collisionScript.OnCollisionEnterGO?.AddListener(OnTriggerFinish);
        }
    }

    private void OnTriggerFinish(GameObject actor)
    {
        if (Utilities.FindParentOfType<ICharacter>(actor.transform, out _) is PlayerScript)
            playerHasPassed = true;
        else if (Utilities.FindParentOfType<ICharacter>(actor.transform, out _) is EscorteeScript)
            escorteeHasPassed = true;
        else
            return;

        if (playerHasPassed && escorteeHasPassed)
            lvlManager.TryEndLevel(true);
    }
}
