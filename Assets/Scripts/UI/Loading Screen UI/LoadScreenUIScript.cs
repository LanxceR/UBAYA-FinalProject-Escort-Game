using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The load screen UI script
/// </summary>
public class LoadScreenUIScript : MonoBehaviour
{
    // Components
    [Header("UI Components")]
    [SerializeField]
    internal Slider loadProgressBar;

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.gameScene)
        {
            loadProgressBar.value = GameManager.Instance.gameScene.LoadProgress * 100f;

            // TODO: Implement other loading screen behaviours here (Tips, Map Screen, etc)
        }
    }
}
