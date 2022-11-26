using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The seek target script (actively seek for target automatically)
/// </summary>
public class SeekTargetScript : MonoBehaviour
{
    // Variables
    [Header("Main Settings")]
    [SerializeField]
    private float radius = 3f;
    [SerializeField]
    private LayerMask aggroLayers = 1 << 6; // ActorBody
    [SerializeField]
    private string[] targetTags;
    [SerializeField]
    private bool overrideReceiveAggro;

    [Header("Targets")]
    [SerializeField]
    internal Transform target;
    [SerializeField]
    internal Collider2D targetCol;

    [Header("Misc Settings")]
    [SerializeField]
    private bool drawGizmo;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Aggro());
    }    

    private WaitForSeconds intervalWait = new WaitForSeconds(1 / 30);
    private IEnumerator Aggro()
    {
        // Loop forever until stopped
        while (true)
        {
            // Wait until the game is not paused
            yield return new WaitUntil(() => GameManager.Instance.GameIsPlaying);

            // Cast OverlapCircle
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius, aggroLayers);
            if (hits.Length <= 1)
            {
                target = null;
                targetCol = null;
            }

            // Compare all hit target tags with targetTags list
            foreach (Collider2D hit in hits)
            {
                foreach (string tag in targetTags)
                {
                    // If found the correct target with the correct tag,
                    if (hit.CompareTag(tag))
                    {
                        // If there are no targets OR the new target is closer
                        if (!target || (hit.transform.position - transform.position).sqrMagnitude < (target.transform.position - transform.position).sqrMagnitude)
                        {
                            TryGetComponent(out ReceiveAggroScript aggro);
                            if (overrideReceiveAggro && aggro)
                            {
                                // Set new target in receive aggro script
                                aggro.ForceAggroTarget(hit.transform, 1f);
                            }
                            else
                            {
                                // Set new target
                                target = hit.attachedRigidbody.transform;
                                targetCol = hit;
                            }
                        }
                    }
                }
            }

            // Perform this aggro detection 30 times / sec
            yield return intervalWait;
        }
    }

#if UNITY_EDITOR
    // Implement this OnDrawGizmosSelected if you want to draw gizmos only if the object is selected
    private void OnDrawGizmos()
    {
        if (!drawGizmo) return;

        // Seek target range (as a wireframe sphere)
        Gizmos.color = new Color(48f / 255, 6f / 255, 0f / 255);
        Gizmos.DrawWireSphere(transform.position, radius);
    }
#endif
}
