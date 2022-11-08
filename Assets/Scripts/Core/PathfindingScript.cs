using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;
using UnityEngine.Events;

[RequireComponent(typeof(Seeker))]
public class PathfindingScript : MonoBehaviour
{
    // Variables
    internal bool isMoving = false;

    // Components
    [Header("Components")]
    private Seeker seeker;
    [SerializeField] Collider2D bodyCollider;

    // Variables
    private Transform target;
    internal Transform Target { get => target; set => target = value; }
    internal Path path;
    internal RaycastHit2D hit;

    internal float distanceToEnd;
    internal int currentWaypoint;

    internal bool targetIsInSight = true; // TODO: Implement line of sight
    internal float lineOfSightCircleCastRadius;

    [Header("Main Settings")]
    [SerializeField] private LayerMask obstacleLayerMask;

    [SerializeField] private float endReachedDistance = 1f; // Useful for ranged characters to stop at a distance to shoot/attack
    
    [SerializeField] private float pathUpdateIntervalSeconds = 0.5f;
    [SerializeField] private float nextWaypointDistance = 0.05f;

    [Header("Misc Settings")]
    [SerializeField] private bool drawGizmo = false;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();

        // Start continuous UpdatePathCoroutine
        StartCoroutine(UpdatePathCoroutine());
    }

    private IEnumerator UpdatePathCoroutine()
    {
        while (true)
        {
            yield return new WaitUntil(() => (this.enabled));

            UpdatePath();

            yield return new WaitForSeconds(pathUpdateIntervalSeconds);
        }
    }

    // Update/generate/calculate a new path
    private void UpdatePath()
    {
        if (seeker.IsDone() && Target)
        {
            // Start path calculation
            seeker.StartPath(transform.position, Target.position, OnPathGenComplete);
        }
    }

    // Method to call when a path is generated/calculated
    private void OnPathGenComplete(Path p)
    {
        if (!p.error)
        {
            // Set path
            path = p;
            // Reset waypoint progress along the new path
            currentWaypoint = 0;
        }
    }

    // Follow the generated path(s)
    internal Vector2 GetDirectionToFollowPath()
    {
        // If there's no path, return
        if (path == null) return Vector2.zero;

        if (bodyCollider)
            lineOfSightCircleCastRadius = bodyCollider.bounds.size.x > bodyCollider.bounds.size.y ? bodyCollider.bounds.size.x / 2 : bodyCollider.bounds.size.y / 2;
        else
            lineOfSightCircleCastRadius = 0.1f;
        IsTargetInLineOfSight(lineOfSightCircleCastRadius);

        // Calculcate distance to end destination
        distanceToEnd = Vector2.Distance(transform.position, Target.position);

        // Has this object reached the destination yet (within this endReachedDistance radius, OR has reached the last waypoint in this path)
        if ((distanceToEnd <= endReachedDistance && targetIsInSight) || (currentWaypoint >= path.vectorPath.Count))
        {
            return Vector2.zero;
        }

        // Set direction for AI movement
        Vector2 dir = ((Vector2)path.vectorPath[currentWaypoint] - (Vector2)transform.position).normalized;

        UpdateCurrentWaypoint();

        return dir;
    }

    // Has this object reached the current waypoint yet? 
    internal void UpdateCurrentWaypoint()
    {
        // (using a distance threshold nextWayPointDistance)
        float distanceToWaypoint = Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);
        if (distanceToWaypoint < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    // TODO: Implement line of sight checking using cone of sight instead
    // Check if target is in gameobject's "line of sight"
    private void IsTargetInLineOfSight(float radius)
    {
        // Perform a raycast from transform position to target's position
        hit = Physics2D.CircleCast(transform.position, radius, DirectionToTarget(), distanceToEnd, obstacleLayerMask);

        // If raycast hit something, that means there's an obstacle in the way
        // If there's an obstacle in the way, target is NOT in line of sight
        if (hit)
            targetIsInSight = false;
        else
            targetIsInSight = true;
    }

    // Find direction in Vector3 to the target
    private Vector3 DirectionToTarget()
    {
        return Target.position - transform.position;
    }

    // Implement this OnDrawGizmosSelected if you want to draw gizmos only if the object is selected
    private void OnDrawGizmos()
    {
        if (!drawGizmo) return;

        // Line of sight
        if (targetIsInSight)
            Gizmos.color = Color.green;
        else
            Gizmos.color = Color.red;
        if (Target)
        {
            Gizmos.DrawLine(transform.position, Target.position);
            Gizmos.DrawWireSphere(transform.position, lineOfSightCircleCastRadius);
        }

        // Stop distance
        Gizmos.color = new Color(166f / 255, 0f / 255, 255f / 255);
        Gizmos.DrawWireSphere(transform.position, endReachedDistance);
    }
}
