using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Many useful methods and fucntions
/// </summary>
public static class Utilities
{
    /// <summary>
    /// Compare if two lists have the same contents regardless of order/index
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="listA"></param>
    /// <param name="listB"></param>
    /// <returns></returns>
    public static bool IsListContentEquals<T>(List<T> listA, List<T> listB)
    {
        if (listA.Count != listB.Count)
            return false;

        for (int n = 0; n < listA.Count; n++)
        {
            bool found = false;
            for (int m = 0; m < listB.Count; m++)
            {
                if (EqualityComparer<T>.Default.Equals(listA[n], listB[m]))
                {
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Compare if two lists have the exact same contents (taking order/index into account)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="listA"></param>
    /// <param name="listB"></param>
    /// <returns></returns>
    public static bool IsListEquals<T>(List<T> listA, List<T> listB)
    {
        if (listA.Count != listB.Count)
            return false;
        for (int i = 0; i < listB.Count; i++)
        {
            if (!EqualityComparer<T>.Default.Equals(listA[i], listB[i]))
                return false;
        }
        return true;
    }

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
    public static GameObject FindParentWithTag(GameObject childObject, string tag, out Transform last)
    {
        Transform t = childObject.transform;
        while (t.parent != null)
        {
            if (t.parent.tag == tag)
            {
                last = t.parent;
                return t.parent.gameObject;
            }
            t = t.parent.transform;
        }

        // Could not find a parent with given tag.
        last = t;
        return null; 
    }
    // Climb up the hirearchy and find a parent with the specified type
    public static Transform FindParent<T>(Transform child, out Transform last) where T: class
    {
        Transform t = child;
        while (t.parent != null)
        {
            if (t.parent.TryGetComponent(out T _))
            {
                last = t.parent;
                return t.parent;
            }
            t = t.parent;
        }

        // Could not find a parent with implementing ICharacter
        last = t;
        return null;
    }
    // Climb up the hirearchy and find a parent with the specified type
    public static T FindParentOfType<T>(Transform child, out Transform last) where T : class
    {
        Transform t = child;
        while (t.parent != null)
        {
            if (t.parent.TryGetComponent(out T parent))
            {
                last = t.parent;
                return parent;
            }
            t = t.parent;
        }

        // Could not find a parent with implementing ICharacter
        last = t;
        return null;
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
