using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyEscorteeAttackScript : MonoBehaviour
{
    // Reference to the main enemy script
    [SerializeField]
    private AllyEscorteeScript allyEscorteeScript;

    // Variables
    [SerializeField]
    private Transform aimHelper;
    [SerializeField]
    private WeaponScript weapon;

    [Header("Stats Settings")]
    [SerializeField] internal float attackDelay = 0.2f; // Delay before executing attack
    [SerializeField] internal float attackCooldown = 3f; // Attack cooldown (attack freq)
    [SerializeField] private float executeAttackRange;

    [Header("Misc Settings")]
    [SerializeField] private bool drawGizmo = false;

    // Variables
    private float cooldown = 0f;
    private bool canAttack = true;

    // Start is called before the first frame update
    void Start()
    {
        // Get weapon range
        executeAttackRange = (weapon.weaponAttackScript as WeaponRangedAttackScript).range;
    }

    // Update is called every frame, if the MonoBehaviour is enabled
    private void Update()
    {
        if (!GameManager.Instance.GameIsPlaying) return;

        // Countdown cooldown until zero
        cooldown = cooldown - Time.deltaTime > 0 ? cooldown - Time.deltaTime : 0f;

        if (allyEscorteeScript.seekTargetScript)
        {
            if (allyEscorteeScript.seekTargetScript.target)
            {
                Transform target = allyEscorteeScript.seekTargetScript.target;
                Collider2D targetCol = allyEscorteeScript.seekTargetScript.targetCol;

                // Aims toward target
                if (targetCol)
                {
                    Vector2 aimPos = targetCol.ClosestPoint(transform.position);
                    aimHelper.position = aimPos;
                }
                else
                {
                    aimHelper.position = target.position;
                }

                // Range detection
                // Use layerMask ActorHitbox ( 1 << 7 )
                RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, aimHelper.position - transform.position, executeAttackRange, 1 << 7);

                foreach (RaycastHit2D h in hits)
                {
                    if (h.transform == target)
                    {
                        // Attack
                        ControlAttackWithAnim();
                    }
                }
            }
        }
    }

    public void ControlAttackWithAnim()
    {
        if (cooldown <= 0f)
        {
            StartCoroutine(AttackCoroutine());
        }
    }

    private IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(attackDelay);

        weapon.weaponAttackScript.BeginAttack();

        // Set Cooldown
        cooldown = attackCooldown;
    }

#if UNITY_EDITOR
    // Implement this OnDrawGizmosSelected if you want to draw gizmos only if the object is selected
    private void OnDrawGizmos()
    {
        if (!drawGizmo) return;

        // Weapon range (as a wireframe sphere)
        Gizmos.color = new Color(252f / 255, 194f / 255, 3f / 255);
        Gizmos.DrawWireSphere(transform.position, executeAttackRange);
    }
#endif
}
