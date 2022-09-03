using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Object constantly moving forward using Moveable
/// </summary>
[RequireComponent(typeof(Moveable))]
public class MoveForward : MonoBehaviour
{
    // Components
    private Moveable moveableComp;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        moveableComp = GetComponent<Moveable>();
    }

    // This function is called when the object becomes enabled and active
    // Put this in OnEnable because pooled objects gets enabled/disabled in hirearchy
    private void OnEnable()
    {
        // Move in vector (0,1,0) in respect of rotation (y axis/transform.up)
        moveableComp.SetDirection(transform.up);
    }
}
