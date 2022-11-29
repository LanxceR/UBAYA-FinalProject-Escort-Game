using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

/// <summary>
/// Moveable objects behaviour (for objects that can move under it's own volition)
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class MoveableScript : MonoBehaviour
{
    // Variables
    internal float velocityThisFrame = 1f;

    // Components
    private Vector3 direction;
    internal Rigidbody2D rb;

    // Events
    [Header("Events")]
    internal UnityEvent BeforeMove = new UnityEvent();
    internal UnityEvent AfterMove = new UnityEvent();

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!rb)
            UpdatePosition();
    }

    // This function is called every fixed framerate frame, if the MonoBehaviour is enabled
    private void FixedUpdate()
    {
        if (rb)
            FixedUpdatePosition();
    }

    // Use to regularly update transform position, usually put in Update()
    private void UpdatePosition()
    {
        BeforeMove?.Invoke();
        transform.position = GetNextPosition();
        AfterMove?.Invoke();
    }
    // Use to regularly update rigidbody position with physics, usually put in FixedUpdate()
    private void FixedUpdatePosition()
    {
        BeforeMove?.Invoke();
        rb.MovePosition(GetNextPosition());
        AfterMove?.Invoke();
    }

    // Get the next position according to direction
    internal Vector3 GetNextPosition()
    {
        // TODO: Fix movement jitter when moving camera using Cinemachine & Pixel Perfect Camera
        Vector3 pos;
        pos.x = (float)Math.Round(transform.position.x + GetDirectionWithVelocity().x, 2);
        pos.y = (float)Math.Round(transform.position.y + GetDirectionWithVelocity().y, 2);
        pos.z = (float)Math.Round(transform.position.z + GetDirectionWithVelocity().z, 2);

        return pos;
    }

    // Get directions
    // Get direction and return a vector with velocity taken into account
    internal Vector3 GetDirectionWithVelocity()
    {
        if (!rb)
            return direction.normalized * Time.deltaTime * velocityThisFrame;
        else
            return direction.normalized * Time.fixedDeltaTime * velocityThisFrame;
    }
    // Get direction and return a normalized vector (magnitude = 1)
    internal Vector3 GetDirectionNormalized()
    {
        if (!rb)
            return direction.normalized * Time.deltaTime * velocityThisFrame;
        else
            return direction.normalized * Time.fixedDeltaTime * velocityThisFrame;
    }

    // Set movement direction
    internal void SetDirection(Vector3 value)
    {
        direction = value;
    }
    internal void SetDirection(Vector2 value)
    {
        direction.y = value.y;
        direction.x = value.x;
    }
    internal void SetDirection(float x, float y)
    {
        direction.y = y;
        direction.x = x;
    }

    // Stop moving
    internal void StopMoving()
    {
        SetDirection(Vector2.zero);
    }

    // Set specific axis direction
    internal void SetXDirection(float xValue)
    {
        direction.x = xValue;
    }
    internal void SetYDirection(float yValue)
    {
        direction.y = yValue;
    }

    // Skew direction by adding a new direction component
    internal void AddDirection(Vector3 value)
    {
        direction += value;
    }
    internal void AddDirection(Vector2 value)
    {
        direction.y += value.y;
        direction.x += value.x;
    }
    internal void AddDirection(float x, float y)
    {
        direction.y += y;
        direction.x += x;
    }
}
