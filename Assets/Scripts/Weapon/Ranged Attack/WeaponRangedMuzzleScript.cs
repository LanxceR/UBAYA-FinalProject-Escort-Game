using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
    [SerializeField]
    private RandomizeSprites muzzleFX;

    // Settings
    [Header("Prefab Type")]
    [SerializeField] private PoolObjectType projectileType; //Type of projectile to fire

    // Fire projectile(s) from this muzzleScripts stored muzzles/fire positions
    public void SpawnProjectile(PoolObjectType poolObjectType, float range, float damage, float velocity, float knockbackForce, float spread,
                                int pierceAmount, float pierceMultiplier, float minPierceMultiplier,
                                GameObject attacker)
    {
        foreach (GameObject muzzle in muzzles)
        {
            SpawnProjectile(muzzle, poolObjectType, range, damage, velocity, knockbackForce, spread, pierceAmount, pierceMultiplier, minPierceMultiplier, attacker);
        }       
    }
    // Fire projectile(s) from this muzzleScripts stored muzzle
    public void SpawnProjectile(GameObject muzzle, PoolObjectType poolObjectType, float range, float damage, float velocity, float knockbackForce, float spread,
                                int pierceAmount, float pierceMultiplier, float minPierceMultiplier,
                                GameObject attacker)
    {
        // Request an object pool
        PoolObject poolObj = ObjectPooler.GetInstance().RequestObject(poolObjectType);

        // Activate fetched object
        ProjectileScript projectile = poolObj.Activate(muzzle.transform.position, Quaternion.identity).GetComponent<ProjectileScript>();

        // Offset projectile spawn position (to take into account sprite sort point)
        projectile.transform.position += projectile.spawnOffset;

        // Randomize the deviation of this muzzle
        float deviation = Random.Range(-spread / 2, spread / 2);

        // Calculate rotation with given spread deviation
        Quaternion muzzleRotation = GetMuzzleRotation(muzzle);
        Quaternion rotation = Quaternion.Euler(muzzleRotation.eulerAngles.x, muzzleRotation.eulerAngles.y, muzzleRotation.eulerAngles.z + deviation);

        // Set projectile (and it's collider) rotation
        projectile.projectileAnimationScript.projectileModel.transform.rotation = rotation;
        projectile.collisionScript.col.transform.rotation = rotation;

        // Set projectile direction
        Vector2 direction = Quaternion.Euler(0, 0, deviation) * GetMuzzleDirection(muzzle);
        projectile.projectileMovementScript.SetDirection(direction);

        // Stats for projectile
        projectile.SetDamage(damage);
        projectile.SetRange(range);
        projectile.projectileMovementScript.SetSpeed(velocity);
        projectile.SetKnockbackForce(knockbackForce);
        projectile.projectileHitScript.SetAttacker(attacker);
        projectile.SetPierce(pierceAmount, pierceMultiplier, minPierceMultiplier);

        // Check for a point blank shot
        List<RaycastHit2D> hitsAtPointBlank = IsThereSomethingShotPointBlank(projectile, muzzle, attacker);
        if (hitsAtPointBlank != null || hitsAtPointBlank.Count > 0 || !hitsAtPointBlank.Any())
        {
            int hits = 0;
            // Point blank shot according to projectile's pierce, ordered from closest
            for (int i = 0; i <= pierceAmount; i++)
            {
                // If this n'th iteration is still in the pierce amount range (to prevent IndexOutOfBoundsExceptions)
                if (i < hitsAtPointBlank.Count)
                {
                    // Call collision enter events from projectile "hitting" the victim
                    projectile.collisionScript.CollisionEnter(hitsAtPointBlank[i].collider.gameObject);

                    // If max pierce amount reached
                    if (i >= pierceAmount)
                    {
                        // Deactivate projectile now (because shot was taken at point blank and "pierced" enough victims)
                        poolObj.Deactivate();
                        return;
                    }
                }
            }
        }
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
                SpawnProjectile(projectileType, w.range, w.damage, w.velocity, w.knockbackForce, w.spread,
                                w.pierceAmount, w.pierceMultiplier, w.minPierceMultiplier,
                                weaponScript.parentHolder);

                StartCoroutine(MuzzleFXCoroutine(0.1f));

                yield return new WaitForSeconds(w.burstDelay);
            }
        }
        else
        {
            // Shoot once
            SpawnProjectile(projectileType, w.range, w.damage, w.velocity, w.knockbackForce, w.spread,
                                 w.pierceAmount, w.pierceMultiplier, w.minPierceMultiplier,
                                 weaponScript.parentHolder);

            StartCoroutine(MuzzleFXCoroutine(0.1f));
        }
    }

    internal IEnumerator MuzzleFXCoroutine(float duration)
    {
        if (!muzzleFX) yield break;

        muzzleFX.gameObject.SetActive(true);

        yield return new WaitForSeconds(duration);

        muzzleFX.gameObject.SetActive(false);
    }

    private List<RaycastHit2D> IsThereSomethingShotPointBlank(ProjectileScript projectileInfo, GameObject muzzle, GameObject attacker)
    {
        List<RaycastHit2D> victims = new List<RaycastHit2D>();

        // Setup            
        var w = weaponScript;
        float distanceToMuzzle = Vector2.Distance(w.parentAttach.transform.position, muzzle.transform.position);
        LayerMask layerMask = LayerMask.GetMask("ActorHitbox");

        // Check for all hits using raycast
        RaycastHit2D[] hits = Physics2D.RaycastAll(w.parentAttach.transform.position, GetMuzzleDirection(muzzle), distanceToMuzzle, layerMask);

        for (int i = 0; i < hits.Length; ++i)
        {
            // NOTE: RaycastHit2D has a property of the rigidbody, so use that instead!
            // If there's a valid hit (with the valid target tags in the projectile),
            if (hits[0].transform.gameObject != attacker && projectileInfo.collisionScript.CheckTargetedTags(hits[i].collider.gameObject) != null)
            {
                // It IS a point blank shot
                victims.Add(hits[i]);
            }
        }

        // Return all hit victims
        return victims;
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
    // Calculate muzzle direction
    private Vector2 GetMuzzleDirection(GameObject muzzle)
    {
        Vector2 direction = GetMuzzleRotation(muzzle) * Vector2.up;
        return direction;
    }
}
