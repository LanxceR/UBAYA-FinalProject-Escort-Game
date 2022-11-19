using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public enum SceneName
{
    TITLE_SCREEN,
    MAIN_HUB,
    TEST_ESCORT_SCENE,
    TEST_MISSION_SCENE
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
    //internal UnityAction<float> OnSceneChange;

    // Variables
    [SerializeField] private string defaultSceneTarget;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // Manage GameManager's components at start
        //ManageGMComponents();

        //OnSceneChange += ManageGMComponents;
        SceneManager.sceneLoaded += ManageGMComponents;
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
        yield return new WaitForSecondsRealtime(delay);

        /** Transition animation / load screen
        // Transition Animation
        animator.SetTrigger("Entrance");

        yield return new WaitForSecondsRealtime(1f);
        */

        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync((int)scene);
        yield return new WaitUntil(() => (asyncLoadLevel.isDone));

        yield return new WaitForEndOfFrame();

        // Reset Timescale
        gameManager.gameState.ResumeGame();

        // Invoke OnSceneChange event
        //OnSceneChange?.Invoke(delay);
    }
    private IEnumerator LoadSceneCoroutine(string sceneName, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);

        /** Transition animation / load screen
        // Transition Animation
        animator.SetTrigger("Entrance");

        yield return new WaitForSecondsRealtime(1f);
        */

        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync(sceneName);
        yield return new WaitUntil(() => (asyncLoadLevel.isDone));

        yield return new WaitForEndOfFrame();

        // Reset Timescale
        gameManager.gameState.ResumeGame();

        // Invoke OnSceneChange event
        //OnSceneChange?.Invoke(delay);
    }
    #endregion

    #region Component Manager
    private void OnSceneLoaded()
    {
        SceneName currentScene = (SceneName)GetCurrentScene().buildIndex;

        switch (currentScene)
        {
            case SceneName.TITLE_SCREEN | SceneName.MAIN_HUB | SceneName.TEST_MISSION_SCENE:
                gameManager.gameState.enabled = false;
                gameManager.gamePlayer.enabled = false;
                gameManager.gameEscortee.enabled = false;
                gameManager.gameWeapon.enabled = false;
                gameManager.gameEnemy.enabled = false;
                break;
            case SceneName.TEST_ESCORT_SCENE:
                gameManager.gameState.enabled = true;
                gameManager.gamePlayer.enabled = true;
                gameManager.gameEscortee.enabled = true;
                gameManager.gameWeapon.enabled = true;
                gameManager.gameEnemy.enabled = true;

                // Find active in-game cameras & UI (if one exists)
                gameManager.FindActiveInGameCameras();
                gameManager.FindActiveInGameUI();
                // Try to initialize In-Game UI & Camera
                gameManager.TryInitializeInGameCameras();
                gameManager.TryInitializeInGameUI();

                // Find any preexisting players first
                gameManager.gamePlayer.FindPlayer();
                // TODO: Spawn player using other method (such as after generating the map)
                // Attempt to spawn Player
                gameManager.gamePlayer.TrySpawnPlayer();

                // Find any preexisting escortees first
                gameManager.gameEscortee.FindEscorteeInScene();
                // TODO: Spawn escortee using other method (such as after generating the map)
                // Attempt to spawn Player
                //gameManager.gameEscorteeSpawnEscortee();
                break;
            default:
                gameManager.gameState.enabled = false;
                gameManager.gamePlayer.enabled = false;
                gameManager.gameEscortee.enabled = false;
                gameManager.gameInput.enabled = true;
                break;
        }
    }
    private void ManageGMComponents(Scene scene, LoadSceneMode loadSceneMode)
    {
        OnSceneLoaded();
    }
    #endregion
}
