using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The emitting aggro script (handles all emitting aggresion)
/// </summary>
public class EmitAggroScript : MonoBehaviour
{
    // Variables
    [Header("Main Settings")]
    [SerializeField]
    private float radius = 3f;
    [SerializeField]
    private LayerMask aggroLayers = 1 << 6;
    [SerializeField]
    private string[] targetTags;

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

            // Compare all hit target tags with targetTags list
            foreach (Collider2D hit in hits)
            {
                Utilities.FindParent<ICharacter>(hit.transform).TryGetComponent(out ReceiveAggroScript aggro);

                foreach (string tag in targetTags)
                {
                    // If found the correct target with the correct tag, activate the target's aggro
                    if (hit.CompareTag(tag))
                    {
                        if (!aggro.active)
                            aggro.active = true;
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

        // Weapon range (as a wireframe sphere)
        Gizmos.color = new Color(90f / 255, 158f / 255, 103f / 255);
        Gizmos.DrawWireSphere(transform.position, radius);
    }
#endif
}
