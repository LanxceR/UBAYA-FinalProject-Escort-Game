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
    [Header("Health")]
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;
    [SerializeField] private bool hasInitializedHealth;
    [Header("Death")]
    [SerializeField] private bool destroyOnDeath;
    [SerializeField] private float destroyDelay;
    [SerializeField] private GameObject corpsePrefab;

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

    internal GameObject lastHitBy;

    // Update is called every frame, if the MonoBehaviour is enabled
    private void Update()
    {
        // If health is not initialized yet
        if (MaxHealth > 0 && CurrentHealth <= 0 && !hasInitializedHealth)
        {
            // Set health
            CurrentHealth = MaxHealth;

            // Set initialized flag to true
            hasInitializedHealth = true;
        }

        UpdateState();
    }

    // This function is called when the object becomes enabled and active
    private void OnEnable()
    {
        Ressurect();
    }

    // Increase health but not past max health
    internal void Heal(float value)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth + value, 0, MaxHealth);
    }

    // Reset health back to max health
    internal void Ressurect()
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

            if (gameObject.name.Contains("Zombie"))
            {
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Zombie/Damage");
            }
            else if (gameObject.name.Contains("Player"))
            {
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Survivor/Damage");
            }
            else if (gameObject.name.Contains("Escortee"))
            {
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Convoy/Damage");
            }
            else if (gameObject.name.Contains("Blockade"))
            {
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Blockade/DamageConcrete");
            }
        }
        else if (!IsDead) // Prevent entity from dying again after its already dead
        {
            // Dies

            if (gameObject.name.Contains("Zombie"))
            {
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Zombie/Damage");
            }
            else if (gameObject.name.Contains("Player"))
            {
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Survivor/Damage");
            }
            else if (gameObject.name.Contains("Escortee"))
            {
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Convoy/Damage");
            }
            else if (gameObject.name.Contains("Blockade"))
            {
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Blockade/DamageConcrete");
            }

            IsDead = true;
            OnHealthReachedZero?.Invoke();

            hasInitializedHealth = false;

            if (destroyOnDeath)
                StartCoroutine(DestroyCoroutine(destroyDelay));
        }
    }

    private IEnumerator DestroyCoroutine(float destroyDelay)
    {
        // TODO: Probably find a way to refer to a parent object to make the hirearchy tidier
        yield return new WaitForSeconds(destroyDelay);

        Instantiate(corpsePrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private IEnumerator Invulnerability()
    {
        IsInvulnerable = true;

        yield return new WaitForSeconds(iFramesDuration);

        IsInvulnerable = false;
    }

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
