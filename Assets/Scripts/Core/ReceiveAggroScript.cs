using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The receiving aggro script (handles all receiving aggro detection)
/// </summary>
public class ReceiveAggroScript : MonoBehaviour
{
    [Header("State Settings")]
    [SerializeField]
    internal bool active;
    [SerializeField]
    private bool isForced;

    [Header("Main Settings")]
    [SerializeField]
    private float radius = 10f;
    [SerializeField]
    private LayerMask aggroLayers = 1 << 6; // ActorBody
    [SerializeField]
    private string[] targetTags;

    // Variables
    [Header("Targets")]
    [SerializeField]
    internal Transform target;
    [SerializeField]
    internal Collider2D targetCol;

    [Header("Misc Settings")]
    [SerializeField]
    private bool drawGizmo;

    // Coroutines
    internal Coroutine aggroDetectionCoroutine;
    private Coroutine forcedAggroCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        aggroDetectionCoroutine = StartCoroutine(AggroDetection());
    }

    internal void ForceAggroTarget(Transform target, float duration)
    {
        forcedAggroCoroutine = StartCoroutine(ForcedAggro(target, duration));
    }

    private WaitForSeconds intervalWait = new WaitForSeconds(1 / 30);
    private IEnumerator AggroDetection()
    {
        // Loop forever until stopped
        while (true)
        {
            // Wait until the game is not paused and aggro is active
            yield return new WaitUntil(() => (GameManager.Instance.GameIsPlaying && active));
            
            // Wait until there is no forced aggro
            yield return new WaitWhile(() => (isForced));

            // Cast OverlapCircle
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius, aggroLayers);

            // Compare all hit target tags with targetTags list
            foreach (Collider2D hit in hits)
            {
                Utilities.FindParent<ICharacter>(hit.transform, out _).TryGetComponent(out EmitAggroScript aggressor);

                foreach (string tag in targetTags)
                {
                    // If found the correct target with the correct tag,
                    if (hit.CompareTag(tag))
                    {
                        // If there are no targets OR the new target is closer
                        if (!target || (hit.transform.position - transform.position).sqrMagnitude < (target.transform.position - transform.position).sqrMagnitude)
                        {
                            // Set new target
                            target = aggressor.transform;
                            targetCol = hit;
                        }
                    }
                }
            }

            // Perform this aggro detection 30 times / sec
            yield return intervalWait;
        }
    }

    private IEnumerator ForcedAggro(Transform target, float duration)
    {
        isForced = true;

        // Set new target
        this.target = target.transform;

        // Set new target collider
        Collider2D[] cols = target.GetComponentsInChildren<Collider2D>();
        this.targetCol = cols[0];
        foreach (Collider2D c in cols)
        {
            if (c.gameObject.layer == 6) this.targetCol = c;
        }

        yield return new WaitForSeconds(duration);

        isForced = false;
    }

#if UNITY_EDITOR
    // Implement this OnDrawGizmosSelected if you want to draw gizmos only if the object is selected
    private void OnDrawGizmos()
    {
        if (!drawGizmo) return;

        // Lose aggro range (as a wireframe sphere)
        Gizmos.color = new Color(48f / 255, 6f / 255, 0f / 255);
        Gizmos.DrawWireSphere(transform.position, radius);
    }
#endif
}
