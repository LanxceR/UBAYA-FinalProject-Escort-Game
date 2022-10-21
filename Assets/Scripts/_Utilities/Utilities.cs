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

    public static Vector2 VectorBetweenTwoPoints(Vector2 first, Vector2 second)
    {
        return first - second;
    }

    // Useful to access a layermask from the layer collision matrix (DOESNT WORK)
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
}
