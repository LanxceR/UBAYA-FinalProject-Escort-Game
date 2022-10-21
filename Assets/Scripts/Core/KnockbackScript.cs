using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// The knockback script
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class KnockbackScript : MonoBehaviour
{
    // Events
    [Header("Events")]
    internal UnityEvent OnKnockback = new UnityEvent();
    internal UnityEvent<float> KnockbackRecovery = new UnityEvent<float>();

    // Components
    private Rigidbody2D rb;
    private MoveableScript moveable;

    // Settings
    [Header("Main Settings")]
    [SerializeField] private PhysicsMaterial2D physMaterial; // Physics material non-moveable objects

    internal bool knockbackImmune = false;
    [SerializeField] private float knockbackResist = 0f;
    [SerializeField] private float recoveryTime = 1f;
    [SerializeField] private float staggerTime = 1f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Try to get moveable component if there's any (not every object can move on it's own volition)
        TryGetComponent(out moveable);
    }

    // Do knockback to this object
    public void DoKnockback(float force, Vector2 direction, bool canResist, bool canRecover)
    {
        // If knockback immune, return
        if (knockbackImmune) return;

        // Temporarily disable movement (simulate stagger)
        if (moveable) moveable.enabled = false;

        float finalForce;
        if (canResist)
        {
            // Calculate force with resistance
            finalForce = force - knockbackResist > 0 ? force - knockbackResist : 0;
        }
        else
        {
            finalForce = force;
        }

        // Apply knockback
        rb.AddForce(direction.normalized * (finalForce), ForceMode2D.Impulse);

        if (canRecover)
        {
            // Invoke knockback event and stagger recovery
            OnKnockback?.Invoke();
            StartCoroutine(RecoverFromStagger(staggerTime));
        }
        else
        {
            EnablePhysisMaterial();
        }

        var debugForce = moveable != null ? finalForce - moveable.GetDirectionWithVelocity().magnitude : finalForce;
        Debug.Log($"{gameObject.name} took {debugForce} knockback");
    }

    private IEnumerator RecoverFromStagger(float staggerTime)
    {
        yield return new WaitForSeconds(staggerTime);

        // Allow for movement again
        if (moveable) moveable.enabled = true;

        // Recover from knockback
        KnockbackRecovery?.Invoke(recoveryTime);
    }

    private void EnablePhysisMaterial()
    {
        rb.sharedMaterial = physMaterial;
    }

    private void DisablePhysisMaterial()
    {
        rb.sharedMaterial = null;
    }
}
