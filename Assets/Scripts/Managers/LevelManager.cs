using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The level manager script
/// </summary>
public class LevelManager : MonoBehaviour
{
    #region Components
    //=============================================================//

    [Header("World Bounds")]
    [SerializeField] private CompositeCollider2D worldBound;

    //=============================================================//

    [Header("Triggers")]
    [SerializeField] private EdgeCollider2D spawnTriggerPrefab;
    [SerializeField] private EdgeCollider2D bigSpawnTriggerPrefab;
    [SerializeField] private EdgeCollider2D finishTriggerPrefab;

    [SerializeField] private GameObject triggersParent;

    //=============================================================//

    [Header("Blockade Prefabs")]
    [SerializeField] private GameObject blockadePrefab;

    [SerializeField] private GameObject blockadeParent;

    //=============================================================//

    [Header("Map Setting")]
    [SerializeField] private GameObject map;
    [SerializeField] private Bounds mapBounds;
    #endregion

    #region Variables
    [Header("Triggers In Scene")]
    [SerializeField] private List<EdgeCollider2D> spawnTriggers;
    [SerializeField] private List<EdgeCollider2D> bigSpawnTriggers;
    [SerializeField] private EdgeCollider2D finishTrigger;

    [Header("In-Game Values")]
    [SerializeField] internal float timePassed;
    internal Coroutine timerCoroutine;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // Assign Main Camera CinemachineConfiner2D's bounding shape
        GameManager.Instance.InGameCameras.mainCameraConfiner2D.m_BoundingShape2D = worldBound;

        SetupLevel();

        if (timerCoroutine == null)
        {
            timerCoroutine = StartCoroutine(TimerCoroutine());
        }
    }

    private void SetupLevel()
    {
        // For now, roughly divide the amount of idle, small wave spawns, and big wave spawns by these proportions
        // Idle -> 10% of totalEnemy
        // Small Wave (Total) -> 40% of totalEnemy
        // Big Wave (Total) -> 50% of totalEnemy

        Vector2 pos;
        float padding = 5f; // Padding area for triggers to always spawn within the padded area (from left OR right)

        // Randomize spawn trigger amounts
        int spawnTriggerAmount = Random.Range(5, 11);
        pos = map.transform.position;
        for (int i = 0; i < spawnTriggerAmount; i++)
        {
            // Randomize spawn trigger position
            // To roughly distribute evenly, randomize each spawn trigger each in their own area,
            // E.g the 2nd out of 10 triggers position would be randomized between 1/10 to 2/10, so on & so forth
            pos.x = Random.Range(
                (mapBounds.extents.x - (padding * 2)) * i / spawnTriggerAmount,
                (mapBounds.extents.x - (padding * 2)) * (i + 1) / spawnTriggerAmount
                ) + padding;
            spawnTriggers.Add(Instantiate(spawnTriggerPrefab, pos, Quaternion.identity, triggersParent.transform));
        }

        // Randomize big spawn trigger amounts
        int bigSpawnTriggerAmount = Random.Range(2, 5);
        pos = map.transform.position;
        for (int i = 0; i < bigSpawnTriggerAmount; i++)
        {
            // Randomize spawn trigger position
            // To roughly distribute evenly, randomize each spawn trigger each in their own area,
            // E.g the 2nd out of 10 triggers position would be randomized between 1/10 to 2/10, so on & so forth
            pos.x = Random.Range(
                (mapBounds.extents.x - (padding * 2)) * i / bigSpawnTriggerAmount,
                (mapBounds.extents.x - (padding * 2)) * (i + 1) / bigSpawnTriggerAmount
                ) + padding;
            bigSpawnTriggers.Add(Instantiate(bigSpawnTriggerPrefab, pos, Quaternion.identity, triggersParent.transform));
        }

        // Put finish line at the end (half the padding from the right)
        pos = map.transform.position;
        pos.x = mapBounds.extents.x - (padding / 2);
        finishTrigger = Instantiate(finishTriggerPrefab, pos, Quaternion.identity, triggersParent.transform);
    }

    private IEnumerator TimerCoroutine()
    {
        // Loop forever until stopped
        while (true)
        {
            timePassed += Time.deltaTime;
            yield return null;
        }
    }
}
