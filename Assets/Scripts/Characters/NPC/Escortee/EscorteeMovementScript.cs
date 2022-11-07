using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The escortee movement script (handles all escortee movements)
/// </summary>
[RequireComponent(typeof(EscorteeScript), typeof(MoveableScript))]
public class EscorteeMovementScript : MonoBehaviour
{
    // Reference to the main player script
    [SerializeField]
    private EscorteeScript escorteeScript;

    // Components
    private MoveableScript moveableComp;

    // Variables
    [SerializeField]
    private float currentMaxSpeed; // To store current calculated speed in respect to other possible modifiers
    [SerializeField]
    private float actualSpeed; // To store current actual speed instead of player's base speed
    [SerializeField] [Range(0,3)]
    private float speedStage; // Speed stages (0 = stop)

    internal Vector2 dir;

    private Coroutine slowDownCoroutine;
    private Coroutine speedUpCoroutine;
    private Coroutine speedChangeCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("PlayerMovementScript starting");
        moveableComp = GetComponent<MoveableScript>();
        currentMaxSpeed = escorteeScript.maxSpeed;
    }

    // This function is called every fixed framerate frame, if the MonoBehaviour is enabled
    // Move Escortee rigidbody in FixedUpdate
    private void FixedUpdate()
    {
        if (!GameManager.Instance.GameIsPlaying) return;

        float Input_SlowDown = escorteeScript.escorteeInputScript.Input_SlowDown;
        float Input_SpeedUp = escorteeScript.escorteeInputScript.Input_SpeedUp;

        if (Input_SpeedUp == 1)
        {
            if (speedUpCoroutine == null)
                speedUpCoroutine = StartCoroutine(ChangeSpeedStage(true));
        }

        if (Input_SlowDown == 1)
        {
            if (slowDownCoroutine == null)
                slowDownCoroutine = StartCoroutine(ChangeSpeedStage(false));
        }

        // Set direction vector for player movement
        dir = Vector2.right;

        // Set moveable speed
        moveableComp.speed = actualSpeed;

        // Move using moveable
        moveableComp.SetDirection(dir);
    }

    private IEnumerator ChangeSpeedStage(bool speedUp)
    {
        float a;

        // Increment/decrement speedstage
        if (speedUp)
        {
            speedStage++;
            a = escorteeScript.acceleration;
        }
        else
        {
            speedStage--;
            a = escorteeScript.deceleration;
        }

        switch (speedStage)
        {
            case 0:
                StartSpeedChangeCoroutine(0, a); // Stops
                break;
            case 1:
                StartSpeedChangeCoroutine(currentMaxSpeed / 3, a); // 1/3 max speed
                break;
            case 2:
                StartSpeedChangeCoroutine(currentMaxSpeed * 2 / 3, a); // 2/3 max speed
                break;
            case 3:
                StartSpeedChangeCoroutine(currentMaxSpeed, a); // Max speed
                break;
        }

        // While input button is still performed, wait until the next frame
        if (speedUp)
        {
            while (escorteeScript.escorteeInputScript.Input_SpeedUp == 1)
            {
                yield return null;
            }
            speedUpCoroutine = null;
        }
        else
        {
            while (escorteeScript.escorteeInputScript.Input_SlowDown == 1)
            {
                yield return null;
            }
            slowDownCoroutine = null;
        }
    }

    private void StartSpeedChangeCoroutine(float targetSpeed, float acceleration)
    {
        // Interrupt any ongoing speed change coroutine
        if (speedChangeCoroutine != null)
        {
            StopCoroutine(speedChangeCoroutine);
            speedChangeCoroutine = null;
        }

        speedChangeCoroutine = StartCoroutine(ChangeSpeed(targetSpeed, acceleration));
    }

    private IEnumerator ChangeSpeed(float targetSpeed, float acceleration)
    {
        // Store the starting speed
        float initialSpeed = actualSpeed;

        // Calculate the delta velocity (they velocity amount to change)
        float velDelta = targetSpeed - actualSpeed;

        // Calculate the velocity change total time/duration (by dividing delta with acceleration)
        // v ÷ a = t
        float totalTime = velDelta / acceleration;

        // Gradually accelerate / decelerate
        float elapsedTime = 0f;
        while (elapsedTime <= totalTime)
        {
            elapsedTime += Time.deltaTime;

            float t = elapsedTime / totalTime;

            actualSpeed = Mathf.Lerp(initialSpeed, targetSpeed, t);

            yield return null;
        }

        // Snap the actual speed to the target velocity at the end
        actualSpeed = targetSpeed;

        speedChangeCoroutine = null;
    }
}
