using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public enum SceneName
{
    TITLE_SCREEN,
    MAIN_HUB,
    TEST_SCENE
}

/// <summary>
/// The game scene manager script (handles both scene transitions, setups, etc AND act as the game manager component manager) 
/// <para>NOTE: Do NOT disable this component at any time</para>
/// </summary>
public class GameSceneManager : MonoBehaviour
{
    // Reference to the game manager script
    [SerializeField]
    internal GameManager gameManager;

    // Unity Events
    /// <summary>
    /// Event invoked when there's a scene change (accepts float argument acting as the invoke delay)
    /// </summary>
    internal UnityAction<float> OnSceneChange;

    // Variables
    [SerializeField] private string defaultSceneTarget;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // Manage GameManager's components at start
        ManageGMComponents();

        OnSceneChange += ManageGMComponents;
    }

    #region Main Functions
    // Go to scene
    public void GotoScene(SceneName scene)
    {
        StartCoroutine(LoadSceneCoroutine(scene, 0));
    }
    public void GotoScene(string sceneName)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName, 0));
    }
    public void GotoScene()
    {
        StartCoroutine(LoadSceneCoroutine(defaultSceneTarget, 0));
    }

    // Go to scene with delay
    public void GotoSceneWithDelay(SceneName scene, float delay)
    {
        StartCoroutine(LoadSceneCoroutine(scene, delay));
    }
    public void GotoSceneWithDelay(string sceneName, float delay)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName, delay));
    }
    public void GotoSceneWithDelay(float delay)
    {
        StartCoroutine(LoadSceneCoroutine(defaultSceneTarget, delay));
    } 
    #endregion

    #region Utilities
    // Get current scene
    public Scene GetCurrentScene()
    {
        return SceneManager.GetActiveScene();
    }

    // Reload scene
    public void ReloadScene()
    {
        StartCoroutine(LoadSceneCoroutine(GetCurrentScene().name, 0));
    } 
    #endregion

    #region Scene Transitions
    // Load scene coroutine
    private IEnumerator LoadSceneCoroutine(SceneName scene, float delay)
    {
        // Reset Timescale now (IMPORTANT: Coroutines wont run if timescale = 0)
        Time.timeScale = 1f;

        // Invoke OnSceneChange event
        OnSceneChange?.Invoke(delay);

        yield return new WaitForSeconds(delay);

        // Transition Animation
        //animator.SetTrigger("Entrance");

        yield return new WaitForSeconds(1f);
        SceneManager.LoadSceneAsync((int)scene);
    }
    private IEnumerator LoadSceneCoroutine(string sceneName, float delay)
    {
        // Reset Timescale now (IMPORTANT: Coroutines wont run if timescale = 0)
        Time.timeScale = 1f;

        // Invoke OnSceneChange event
        OnSceneChange?.Invoke(delay);

        yield return new WaitForSeconds(delay);

        // Transition Animation
        //animator.SetTrigger("Entrance");

        yield return new WaitForSeconds(1f);
        SceneManager.LoadSceneAsync(sceneName);
    }
    #endregion

    #region Component Manager
    private void ManageGMComponents()
    {
        SceneName currentScene = (SceneName)GetCurrentScene().buildIndex;

        switch (currentScene)
        {
            case SceneName.TITLE_SCREEN | SceneName.MAIN_HUB:
                gameManager.gameState.enabled = false;
                gameManager.gamePlayer.enabled = false;
                gameManager.gameEscortee.enabled = false;
                gameManager.gameData.enabled = true;
                break;
            case SceneName.TEST_SCENE:
                gameManager.gameState.enabled = true;
                gameManager.gamePlayer.enabled = true;
                gameManager.gameEscortee.enabled = true;
                gameManager.gameData.enabled = true;

                // Initialize In-Game UI & Camera
                gameManager.InitializeCameras();
                gameManager.InitializeUI();
                break;
            default:
                gameManager.gameState.enabled = false;
                gameManager.gamePlayer.enabled = false;
                gameManager.gameEscortee.enabled = false;
                gameManager.gameData.enabled = true;
                gameManager.gameInput.enabled = true;
                break;
        }
    }
    private void ManageGMComponents(float delay)
    {
        ManageGMComponents();
    }
    #endregion
}
