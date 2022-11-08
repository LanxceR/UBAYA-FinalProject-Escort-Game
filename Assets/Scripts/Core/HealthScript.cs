using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// The health script
/// </summary>
public class HealthScript : MonoBehaviour
{
    // TODO: Implement various health HUDs

    // Variables
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;

    // Layer masks
    [Header("Layer Masks")]
    [SerializeField] private LayerMask corpseLayerMask = 1 << 10; // Layer Uninteractable

    // Components
    [Header("Components")]
    [SerializeField] private Collider2D[] colliders;

    // Events
    [Header("Events")]
    internal UnityEvent OnHit = new UnityEvent();
    internal UnityEvent OnHealthReachedZero = new UnityEvent();

    internal float MaxHealth { get => maxHealth; set => maxHealth = value; }
    internal float CurrentHealth { 
        get => currentHealth;
        private set {
            currentHealth = value > MaxHealth ? MaxHealth : value <= 0 ? 0 : value;
        } 
    }

    internal bool IsDead { get; private set; }
    internal bool IsInvulnerable { get; private set; }

    private float iFramesDuration;

    private GameObject lastHitBy;

    // Start is called before the first frame update
    void Start()
    {
        // Set health at start
        CurrentHealth = MaxHealth;
    }

    // Update is called every frame, if the MonoBehaviour is enabled
    private void Update()
    {
        UpdateState();
    }

    // This function is called when the object becomes enabled and active
    private void OnEnable()
    {
        FullHeal();
    }

    // Increase health but not past max health
    internal void Heal(float value)
    {
        CurrentHealth += value;
    }

    // Reset health back to max health
    internal void FullHeal()
    {
        CurrentHealth = MaxHealth;
        IsDead = false;
    }

    // Check if the player have max health or not
    internal bool IsOnMaxHealth()
    {
        return currentHealth == maxHealth;
    }

    // Take damage
    public void TakeDamage(GameObject attacker, float damage)
    {
        // Set last hit by attacker
        lastHitBy = attacker;

        // If invincible or dead, return
        if (IsInvulnerable || IsDead) return;

        // Deduct current health by damage
        currentHealth = currentHealth - damage;

        if (currentHealth > 0)
        {
            OnHit?.Invoke();

            if (iFramesDuration > 0)
            {
                // Start I-frames
                StartCoroutine(Invulnerability());
                // TODO: Maybe implement I-Frames blink
            }

            Debug.Log($"{gameObject.name} took {damage} damage from {lastHitBy.name}");
        }
        else if (!IsDead) // Prevent entity from dying again after its already dead
        {
            // Dies
            IsDead = true;
            OnHealthReachedZero?.Invoke();

            // TODO: Handle gameover events (or player death, specifically)
        }
    }

    private IEnumerator Invulnerability()
    {
        IsInvulnerable = true;

        yield return new WaitForSeconds(iFramesDuration);

        IsInvulnerable = false;
    }

    // TODO: Implement State Checking for health (e.g Change corpse layer to Corpse layer to prevent any further interactions). Maybe do this on a separate script?
    private void UpdateState()
    {
        if (IsDead)
        {
            // Set corpse's colliders to the corpse layer
            foreach (var c in colliders)
            {
                c.gameObject.layer = Utilities.ToLayer(corpseLayerMask.value);
            }
        }
        /*
        else if (isInvulnerable)
        {
            gameObject.layer = Utilities.ToLayer(invulnerableLayerMask.value);
        }
        else
        {
            gameObject.layer = Utilities.ToLayer(defaultLayerMask.value);
        }
        */
    }
}
