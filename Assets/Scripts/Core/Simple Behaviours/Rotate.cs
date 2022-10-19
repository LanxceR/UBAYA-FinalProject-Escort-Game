using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    private Quaternion defaultRotation;
    private bool rotating = true; // To start & stop rotation

    // Continuous rotation
    [Header("Main Settings")]
    [SerializeField] private bool flipRotation;

    [Header("Free Rotation Settings")]
    [SerializeField] private float speedInRotation;

    // Constrained rotation (back and forth effect)
    [Header("Constrained Rotation Settings")]
    [SerializeField] private bool constrainRotation = false;
    [SerializeField] private float frequency = 0.5f;
    [SerializeField] private float maxRotation = 45f;

    // Start is called before the first frame update
    void Start()
    {
        // Save default rotation
        defaultRotation = transform.rotation;

        // Flip rotation
        if (flipRotation)
            FlipRotation();
    }

    // Update is called once per frame
    void Update()
    {
        if (!rotating) return;

        if (!constrainRotation)
            FreeRotate();
        else
            RestrainedRotate();
    }

    // This function is called when the object becomes enabled and active
    private void OnEnable()
    {
        StartRotation();
    }


    private void FreeRotate()
    {
        transform.Rotate(0, 0, speedInRotation * 360 * Time.deltaTime);
    }

    private void RestrainedRotate()
    {
        transform.localRotation = Quaternion.Euler(0f, 0f, defaultRotation.eulerAngles.z + (maxRotation * Mathf.Sin(Time.fixedTime * frequency * Mathf.PI)));
    }

    private void FlipRotation()
    {
        speedInRotation *= -1;
        maxRotation *= -1;
    }

    // Reset rotation back to identity
    public void ResetRotation()
    {
        transform.rotation = Quaternion.identity;
    }

    // Stop rotation
    public void StopRotation()
    {
        rotating = false;
    }
    // Stop rotation
    public void StartRotation()
    {
        rotating = true;
    }
}
