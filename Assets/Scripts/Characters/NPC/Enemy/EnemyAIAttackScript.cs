using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The enemy attacking script (handles all enemy auto attacking actions)
/// </summary>
public class EnemyAIAttackScript : MonoBehaviour
{
    // Reference to the main enemy script
    [SerializeField]
    private EnemyScript enemyScript;

    // Variables
    [SerializeField]
    private Transform aimHelper;
    [SerializeField]
    private WeaponScript weapon;

    [Header("Stats Settings")]
    [SerializeField] internal float attackDelay = 1f;
    [SerializeField] private float executeAttackRange = 0.16f; // Melee range

    [Header("Misc Settings")]
    [SerializeField] private bool drawGizmo = false;

    // Variables
    private float cooldown = 0f;
    private bool canAttack = true;

    // Update is called once per frame
    void Update()
    {        
        if (!GameManager.Instance.GameIsPlaying) return;

        // Countdown cooldown until zero
        cooldown = cooldown - Time.deltaTime > 0 ? cooldown - Time.deltaTime : 0f;

        if (enemyScript.recAggroScript)
        {
            if (enemyScript.recAggroScript.target)
            {
                Transform target = enemyScript.recAggroScript.target;
                Collider2D targetCol = enemyScript.recAggroScript.targetCol;

                // TODO: Aim towards the target's hitbox closest point instead

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
            enemyScript.enemyAnimationScript.AttackAnimation();
            weapon.weaponAttackScript.BeginAttack();

            // Set Cooldown
            cooldown = attackDelay;
        }
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
