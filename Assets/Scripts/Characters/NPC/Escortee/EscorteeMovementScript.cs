using System;
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
    internal MoveableScript moveableComp;

    // Variables
    [SerializeField]
    private float currentMaxSpeed; // To store current calculated speed in respect to other possible modifiers
    [SerializeField]
    internal float actualSpeed; // To store current actual speed instead of player's base speed
    [SerializeField] [Range(0,3)]
    internal float speedStage; // Speed stages (0 = stop)

    internal Vector2 dir;

    private Coroutine slowDownCoroutine;
    private Coroutine speedUpCoroutine;
    private Coroutine speedChangeCoroutine;


    private FMOD.Studio.EventInstance instance;

    private float xPosPlayer;
    private float yPosPlayer;
    private Vector3 audioPoint;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("PlayerMovementScript starting");
        moveableComp = GetComponent<MoveableScript>();
        currentMaxSpeed = escorteeScript.maxSpeed;

        // Add listener to Health's OnHealthReachedZero UnityEvent
        escorteeScript.healthScript.OnHealthReachedZero.AddListener(EscorteeDeath);

        DetermineEscortee();
        instance.setParameterByName("RPM", 0);
        instance.start();

        xPosPlayer = GameManager.Instance.gamePlayer.ActivePlayer.transform.position.x;
        yPosPlayer = GameManager.Instance.gamePlayer.ActivePlayer.transform.position.y;
    }

    // This function is called every fixed framerate frame, if the MonoBehaviour is enabled
    // Move Escortee rigidbody in FixedUpdate
    private void FixedUpdate()
    {
        if (!GameManager.Instance.GameIsPlaying) return;

        float Input_SlowDown = escorteeScript.escorteeInputScript.Input_SlowDown;
        float Input_SpeedUp = escorteeScript.escorteeInputScript.Input_SpeedUp;
               
        if (escorteeScript.healthScript)
            if (!escorteeScript.healthScript.IsDead)
            {
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
            }

        // Set direction vector for player movement
        dir = Vector2.right;

        // Set moveable speed
        moveableComp.velocityThisFrame = actualSpeed;

        // Move using moveable
        moveableComp.SetDirection(dir);

        //Sounds
        xPosPlayer = GameManager.Instance.gamePlayer.ActivePlayer.transform.position.x - GameManager.Instance.gameEscortee.ActiveEscortee.transform.position.x;
        yPosPlayer = GameManager.Instance.gamePlayer.ActivePlayer.transform.position.y - GameManager.Instance.gameEscortee.ActiveEscortee.transform.position.y;

        audioPoint = new Vector3(xPosPlayer * -1, yPosPlayer*-1, GameManager.Instance.gamePlayer.ActivePlayer.transform.position.z - 
            GameManager.Instance.gameEscortee.ActiveEscortee.transform.position.z);

        instance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(audioPoint));
    }

    private void EscorteeDeath()
    {
        // Stop the escortee
        StartSpeedChangeCoroutine(0, escorteeScript.deceleration, true);
    }


    private IEnumerator ChangeSpeedStage(bool speedUp)
    {
        float a;

        // Increment/decrement speedstage
        if (speedUp)
        {
            speedStage = Mathf.Clamp(speedStage + 1, 0, 3);
            a = escorteeScript.acceleration;
        }
        else
        {
            speedStage = Mathf.Clamp(speedStage - 1, 0, 3);
            a = escorteeScript.deceleration;
        }

        switch (speedStage)
        {
            case 0:
                StartSpeedChangeCoroutine(0, a, false); // Stops
                instance.setParameterByName("RPM", 0f);
                break;
            case 1:
                StartSpeedChangeCoroutine(currentMaxSpeed / 4, a, false); // 1/4 max speed
                instance.setParameterByName("RPM", 10f);
                break;
            case 2:
                StartSpeedChangeCoroutine(currentMaxSpeed / 2, a, false); // 1/2 max speed
                instance.setParameterByName("RPM", 20f);
                break;
            case 3:
                StartSpeedChangeCoroutine(currentMaxSpeed, a, false); // Max speed
                instance.setParameterByName("RPM", 30f);
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

    private void StartSpeedChangeCoroutine(float targetSpeed, float acceleration, bool disableMovementAtEnd)
    {
        // Interrupt any ongoing speed change coroutine
        if (speedChangeCoroutine != null)
        {
            StopCoroutine(speedChangeCoroutine);
            speedChangeCoroutine = null;
        }

        speedChangeCoroutine = StartCoroutine(ChangeSpeed(targetSpeed, acceleration, disableMovementAtEnd));
    }

    private IEnumerator ChangeSpeed(float targetSpeed, float acceleration, bool disableMovementAtEnd)
    {
        // Store the starting speed
        float initialSpeed = actualSpeed;

        // Calculate the delta velocity (they velocity amount to change)
        float velDelta = targetSpeed - actualSpeed;

        // Calculate the velocity change total time/duration (by dividing delta with acceleration)
        // v ÷ a = t
        float totalTime = Mathf.Abs(velDelta / acceleration);

        // Gradually accelerate / decelerate
        float elapsedTime = 0f;
        while (elapsedTime <= totalTime)
        {
            elapsedTime += Time.fixedDeltaTime;

            float t = elapsedTime / totalTime;

            actualSpeed = Mathf.Lerp(initialSpeed, targetSpeed, t);

            yield return null;
        }

        // Snap the actual speed to the target velocity at the end
        actualSpeed = targetSpeed;

        // Disable component if permitted
        if (disableMovementAtEnd)
        {
            yield return new WaitForSeconds(0.5f);
            this.enabled = false;
        }

        speedChangeCoroutine = null;
    }

    void DetermineEscortee()
    {
        switch (escorteeScript.id)
        {
            case EscorteeID.BUS:
                instance = FMODUnity.RuntimeManager.CreateInstance("event:/Convoy/BusEngine");
                break;
            case EscorteeID.PICKUP_TRUCK:
                instance = FMODUnity.RuntimeManager.CreateInstance("event:/Convoy/PickupEngine");
                break;
            case EscorteeID.MILITARY_TRUCK:
                instance = FMODUnity.RuntimeManager.CreateInstance("event:/Convoy/CargoEngine");
                break;
        }
    }

    public void KillSound()
    {
        instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        instance.release();
    }
}
