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

    public static Vector2 VectorBetweenTwoPoints(Vector2 first, Vector2 second)
    {
        return first - second;
    }
}
