using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Object Look At behaviours based on target's position using transform.up
/// </summary>
public class LookAt : MonoBehaviour
{
    // Variables
    [SerializeField]
    private Transform target;
    internal Transform Target { get => target; set => target = value; }

    [SerializeField]
    private bool autoAssignTargetInParent = true;

    [SerializeField]
    private string targetTag = "Aim";

    // This function is called when the object becomes enabled and active
    private void OnEnable()
    {
        // Look for aim target in parents
        if (autoAssignTargetInParent)
        {
            // Fetch parent implementing ICharacter
            GameObject parent = Utilities.FindParentOfType<ICharacter>(transform).GetGameObject();
            // Find a child with the correct tag and assign that as target
            Target = Utilities.FindChildWithTag(parent, targetTag).transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.GameIsPlaying) return;

        // Flip sprite based on parent's euler angle (rotation in degrees)
        // 180 < x < 360  ==>  Aiming left
        // 0 < x < 180  ==>  Aiming right
        if (180f < transform.eulerAngles.z && transform.eulerAngles.z < 360f)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {

            transform.localScale = new Vector3(-1, 1, 1);
        }

        // Get target's position
        Vector2 pos = Target.position;

        // "Rotate" this gameobject up axis towards target's position
        transform.up = new Vector3(pos.x - transform.position.x, pos.y - transform.position.y);
    }
}
