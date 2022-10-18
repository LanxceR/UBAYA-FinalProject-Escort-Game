using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The weapon muzzle script (handles all weapon muzzle behaviour (if the weapon has one))
/// </summary>
public class WeaponMuzzleScript : MonoBehaviour
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

    // TODO: Use a raycast from player's center to detect a point blank shot happening (Prevent gun phasing through someone and ending up shooting behind them)
    // Fire projectile(s) from this muzzleScripts stored muzzles/fire positions
    public void SpawnProjectile(PoolObjectType poolObjectType, float range, float damage, float velocity, float knockbackForce, float spread, GameObject attacker)
    {
        foreach (GameObject muzzle in muzzles)
        {
            // Request an object pool
            PoolObject poolObj = ObjectPooler.GetInstance().RequestObject(poolObjectType);

            // Calculate rotation with given spread deviation (randomed)
            Quaternion rotation = Quaternion.Euler(
                muzzle.transform.rotation.eulerAngles.x,
                muzzle.transform.rotation.eulerAngles.y,
                muzzle.transform.rotation.eulerAngles.z + Random.Range(-spread / 2, spread / 2)
                );
            
            // Activate fetched object
            ProjectileScript projectile = poolObj.Activate(muzzle.transform.position, Quaternion.identity).GetComponent<ProjectileScript>();
            
            // Offset projectile spawn position (to take into account sprite sort point)
            projectile.transform.position += projectile.spawnOffset;

            // Set projectile rotation
            projectile.projectileAnimationScript.projectileModel.transform.rotation = rotation;
            projectile.projectileCollisionScript.projectileCollider.transform.rotation = rotation;

            // Set projectile direction
            Vector2 direction = rotation * Vector2.up;
            projectile.projectileMovementScript.SetDirection(direction);

            // Stats for projectile
            projectile.SetDamage(damage);
            projectile.SetRange(range);
            projectile.projectileMovementScript.SetVelocity(velocity);
            projectile.SetKnockbackForce(knockbackForce);
            projectile.projectileHitScript.SetAttacker(attacker);
        }        
    }

    internal IEnumerator ShootCoroutine()
    {
        // For abbreviation
        var w = weaponScript;

        // Check if weapon has a burst fire mechanism
        if (weaponScript.isBurstFire)
        {
            // Shoot in bursts
            for (int i = 0; i < weaponScript.burstAmount; i++)
            {
                SpawnProjectile(projectileType, w.range, w.damage, w.velocity, w.knockbackForce, w.spread, w.parent);                

                yield return new WaitForSeconds(w.burstDelay);
            }
        }
        else
        {
            // Shoot once
            SpawnProjectile(projectileType, w.range, w.damage, w.velocity, w.knockbackForce, w.spread, w.parent);
        }
    }
}
