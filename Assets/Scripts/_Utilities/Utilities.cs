using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Many useful methods and fucntions
/// </summary>
public static class Utilities
{
    // Converts given bitmask to layer number
    public static int ToLayer(int bitmask)
    {
        int result = bitmask > 0 ? 0 : 31;
        while (bitmask > 1)
        {
            bitmask = bitmask >> 1;
            result++;
        }
        return result;
    }

    // Climb up the hirearchy and find a parent with the specified tag
    public static GameObject FindParentWithTag(GameObject childObject, string tag)
    {
        Transform t = childObject.transform;
        while (t.parent != null)
        {
            if (t.parent.tag == tag)
            {
                return t.parent.gameObject;
            }
            t = t.parent.transform;
        }

        // Could not find a parent with given tag.
        return null; 
    }
    // Climb up the hirearchy and find a parent with the specified type
    public static Transform FindParent<T>(Transform child) where T: class
    {
        Transform t = child;
        while (t.parent != null)
        {
            if (t.parent.TryGetComponent(out T _))
            {
                return t.parent;
            }
            t = t.parent;
        }

        // Could not find a parent with implementing ICharacter
        return t;
    }
    // Climb up the hirearchy and find a parent with the specified type
    public static T FindParentOfType<T>(Transform child) where T : class
    {
        Transform t = child;
        while (t.parent != null)
        {
            if (t.parent.TryGetComponent(out T parent))
            {
                return parent;
            }
            t = t.parent;
        }

        // Could not find a parent with implementing ICharacter
        return default(T);
    }

    // Find a child with the specified tag
    public static GameObject FindChildWithTag(GameObject parentObject, string tag)
    {
        Transform[] children = parentObject.GetComponentsInChildren<Transform>(true);

        foreach (Transform child in children)
        {
            if (child.tag == tag)
            {
                return child.gameObject;
            }
        }

        // Could not find a child with given tag.
        return null;
    }

    public static Vector2 VectorBetweenTwoPoints(Vector2 first, Vector2 second)
    {
        return first - second;
    }

    // Useful to access a layermask from the layer collision matrix (DOESN'T WORK)
    private static Dictionary<int, int> _masksByLayer;
    public static void InitializeLayerMatrixMask()
    {
        _masksByLayer = new Dictionary<int, int>();
        for (int i = 0; i < 32; i++)
        {
            int mask = 0;
            for (int j = 0; j < 32; j++)
            {
                if (!Physics.GetIgnoreLayerCollision(i, j))
                {
                    mask |= 1 << j;
                }
            }
            _masksByLayer.Add(i, mask);
        }
    }
    public static int LayerMaskForLayer(int layer)
    {
        InitializeLayerMatrixMask();
        return _masksByLayer[layer];
    }

    public static float GetDirectionAngle(Vector2 dir)
    {
        // Get the angle in degrees (-180, 180)
        float degAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        // Normalize angle (0, 360)
        if (degAngle < 0) degAngle += 360;

        return degAngle;
    }
}
