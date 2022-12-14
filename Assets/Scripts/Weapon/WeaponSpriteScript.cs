using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The weapon sprite script (handles all weapon model rendering)
/// </summary>
public class WeaponSpriteScript : MonoBehaviour
{
    // Reference to the main player script
    [SerializeField]
    private WeaponScript weaponScript;

    // Components
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    // Variables
    private float degRotation;

    // Update is called once per frame
    void Update()
    {
        // Get the rotation angle in degrees (90, 450)
        // add 90 deg because weapons are rotated by 90 deg in prefabs
        degRotation = weaponScript.transform.rotation.eulerAngles.z + 90;

        // Normalize angle (0, 360)
        if (degRotation > 360) degRotation -= 360;

        // Perform direction checking
        if (spriteRenderer)
        {
            if (180 > degRotation && degRotation > 0)
            {
                // Facing back / up
                spriteRenderer.sortingOrder = -1;
            }
            else
            {
                // Facing front / down
                spriteRenderer.sortingOrder = 1;
            }
        }
    }
}
