using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The weapon muzzle script (handles all weapon muzzle behaviour (if the weapon has one))
/// </summary>
[RequireComponent(typeof(WeaponScript), typeof(WeaponRangedAttackScript))]
public class WeaponRangedMuzzleScript : MonoBehaviour
{
    // Reference to the main player script
    [SerializeField]
    private WeaponScript weaponScript;

    // Components
    [SerializeField]
    private List<GameObject> muzzles;

    // Settings
    [Header("Prefab Type")]
    [SerializeField] private PoolObjectType projectileType; //Type of projectile to fire

    // Fire projectile(s) from this muzzleScripts stored muzzles/fire positions
    public void SpawnProjectile(PoolObjectType poolObjectType, float range, float damage, float velocity, float knockbackForce, float spread, GameObject attacker)
    {
        foreach (GameObject muzzle in muzzles)
        {
            // Request an object pool
            PoolObject poolObj = ObjectPooler.GetInstance().RequestObject(poolObjectType);

            // Calculate rotation with given spread deviation (randomed)
            Quaternion rotation = GetMuzzleRotationWithDeviation(muzzle, spread);

            // Activate fetched object
            ProjectileScript projectile = poolObj.Activate(muzzle.transform.position, Quaternion.identity).GetComponent<ProjectileScript>();
                      
            // Offset projectile spawn position (to take into account sprite sort point)
            projectile.transform.position += projectile.spawnOffset;

            // Set projectile (and it's collider) rotation
            projectile.projectileAnimationScript.projectileModel.transform.rotation = rotation;
            projectile.collisionScript.col.transform.rotation = rotation;

            // TODO: Fix projectile direction deviation properly
            // Set projectile direction
            Vector2 direction = GetMuzzleDirection(muzzle);
            projectile.projectileMovementScript.SetDirection(direction);

            // Stats for projectile
            projectile.SetDamage(damage);
            projectile.SetRange(range);
            projectile.projectileMovementScript.SetSpeed(velocity);
            projectile.SetKnockbackForce(knockbackForce);
            projectile.projectileHitScript.SetAttacker(attacker);

            // Check for a point blank shot
            RaycastHit2D hitAtPointBlank = IsThereSomethingShotPointBlank(projectile, muzzle);
            if (hitAtPointBlank)
            {
                // Call collision enter events from projectile "hitting" the victim
                projectile.collisionScript.CollisionEnter(hitAtPointBlank.collider.transform.gameObject);

                // Deactivate projectile (because shot was taken at point blank)
                poolObj.Deactivate();
                return;
            }
        }        
    }
    // Fire projectile(s) from this muzzleScripts stored muzzle
    public void SpawnProjectile(GameObject muzzle, PoolObjectType poolObjectType, float range, float damage, float velocity, float knockbackForce, float spread, GameObject attacker)
    {
        // Request an object pool
        PoolObject poolObj = ObjectPooler.GetInstance().RequestObject(poolObjectType);

        // Calculate rotation with given spread deviation (randomed)
        Quaternion rotation = GetMuzzleRotationWithDeviation(muzzle, spread);

        // Activate fetched object
        ProjectileScript projectile = poolObj.Activate(muzzle.transform.position, Quaternion.identity).GetComponent<ProjectileScript>();

        // Offset projectile spawn position (to take into account sprite sort point)
        projectile.transform.position += projectile.spawnOffset;

        // Set projectile (and it's collider) rotation
        projectile.projectileAnimationScript.projectileModel.transform.rotation = rotation;
        projectile.collisionScript.col.transform.rotation = rotation;

        // Set projectile direction
        Vector2 direction = GetMuzzleDirection(muzzle);
        projectile.projectileMovementScript.SetDirection(direction);

        // Stats for projectile
        projectile.SetDamage(damage);
        projectile.SetRange(range);
        projectile.projectileMovementScript.SetSpeed(velocity);
        projectile.SetKnockbackForce(knockbackForce);
        projectile.projectileHitScript.SetAttacker(attacker);
    }

    // Execute shooting
    internal IEnumerator ShootCoroutine()
    {
        // For abbreviation
        var w = weaponScript.weaponAttackScript as WeaponRangedAttackScript;

        // Check if weapon has a burst fire mechanism
        if (w.isBurstFire)
        {
            // Shoot in bursts
            for (int i = 0; i < w.burstAmount; i++)
            {
                SpawnProjectile(projectileType, w.range, w.damage, w.velocity, w.knockbackForce, w.spread, weaponScript.parentHolder);

                yield return new WaitForSeconds(w.burstDelay);
            }
        }
        else
        {
            // Shoot once
            SpawnProjectile(projectileType, w.range, w.damage, w.velocity, w.knockbackForce, w.spread, weaponScript.parentHolder);
        }
    }

    private RaycastHit2D IsThereSomethingShotPointBlank(ProjectileScript projectileInfo, GameObject muzzle)
    {
        // Setup            
        var w = weaponScript;
        float distanceToMuzzle = Vector2.Distance(w.parentAttach.transform.position, muzzle.transform.position);
        LayerMask layerMask = LayerMask.GetMask("ActorHitbox");

        // Check for all hits using raycast
        RaycastHit2D[] hits = Physics2D.RaycastAll(w.parentAttach.transform.position, GetMuzzleDirection(muzzle), distanceToMuzzle, layerMask);

        for (int i = 0; i < hits.Length; ++i)
        {
            // If there's a valid hit (with the valid target tags in the projectile),
            if (i == 1 && projectileInfo.collisionScript.CheckTargetedTags(hits[i].collider.transform.parent.gameObject) != null)
            {
                // It IS a point blank shot
                return hits[i];
            }
        }

        // Nothing is being shot at point blank
        return new RaycastHit2D();
    }

    // Calculate muzzle rotation
    private Quaternion GetMuzzleRotation(GameObject muzzle)
    {
        Quaternion rotation = Quaternion.Euler(
            muzzle.transform.rotation.eulerAngles.x,
            muzzle.transform.rotation.eulerAngles.y,
            muzzle.transform.rotation.eulerAngles.z
            );
        return rotation;
    }
    // Calculate rotation with given spread deviation (randomed)
    private Quaternion GetMuzzleRotationWithDeviation(GameObject muzzle, float spread)
    {
        Quaternion rotation = Quaternion.Euler(
            muzzle.transform.rotation.eulerAngles.x,
            muzzle.transform.rotation.eulerAngles.y,
            muzzle.transform.rotation.eulerAngles.z + Random.Range(-spread / 2, spread / 2)
            );
        return rotation;
    }
    // Calculate muzzle direction
    private Vector2 GetMuzzleDirection(GameObject muzzle)
    {
        Vector2 direction = GetMuzzleRotation(muzzle) * Vector2.up;
        return direction;
    }
}
