using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// The health script
/// </summary>
public class Health : MonoBehaviour
{
    // Events
    [Header("Events")]
    internal UnityEvent OnHit = new UnityEvent();
    internal UnityEvent OnHealthReachedZero = new UnityEvent();

    // Variables
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;

    public float MaxHealth { get => maxHealth; set => maxHealth = value; }
    public float CurrentHealth { 
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
    /*
    // Perform various update to the object based on different states
    private void UpdateState()
    {
        if (isDead)
        {
            SetSpriteColor(deadColor);
            gameObject.layer = ToLayer(corpseLayerMask.value);
        }
        else if (isInvulnerable)
        {
            gameObject.layer = ToLayer(invulnerableLayerMask.value);
        }
        else
        {
            gameObject.layer = ToLayer(defaultLayerMask.value);
        }
    }
    */

    // Converts given bitmask to layer number
    public static int ToLayer(int bitmask)
    {
        int result = bitmask > 0 ? 0 : 31;
        while (bitmask > 1)
        {
            bitmask = bitmask >> 1;
            result++;
        }
        return result;
    }
}
