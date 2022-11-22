using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public enum SceneName
{
    LOADING_SCREEN,
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

    [Header("Components")]
    [SerializeField] internal CanvasGroup loadTransitionCanvas;

    // Variables
    [Header("Variables")]
    [SerializeField] private string defaultSceneTarget;

    public SceneName SceneToLoad { get; private set; }
    public float LoadProgress { get; private set; }

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // Manage GameManager's components at start
        //ManageGMComponents();

        //OnSceneChange += ManageGMComponents;
        SceneManager.sceneLoaded += ManageGMComponents;
    }

    // Start is called just before any of the Update methods is called the first time
    private void Start()
    {
        if (loadTransitionCanvas)
        {
            loadTransitionCanvas.alpha = 0f;
            loadTransitionCanvas.gameObject.SetActive(false);
        }
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

    // Smoothly fade loading screen into the specified target alpha
    private IEnumerator FadeLoadingScreen(float targetAlpha, float duration)
    {
        float startAlpha = loadTransitionCanvas.alpha;
        float timeElapsed = 0;
        while (timeElapsed < duration)
        {
            loadTransitionCanvas.alpha = Mathf.Lerp(startAlpha, targetAlpha, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        loadTransitionCanvas.alpha = targetAlpha;
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
        LoadProgress = 0f;
        SceneToLoad = scene;
        yield return new WaitForSecondsRealtime(delay);

        /** Transition animation / load screen
        // Transition Animation
        animator.SetTrigger("Entrance");

        yield return new WaitForSecondsRealtime(1f);
        */

        // Activate laoding screen
        loadTransitionCanvas.gameObject.SetActive(true);
        // Fade in loading screen for 1 second
        yield return StartCoroutine(FadeLoadingScreen(1f, 1f));
        // Load loading screen scene
        AsyncOperation asyncLoadingScreen = SceneManager.LoadSceneAsync((int)SceneName.LOADING_SCREEN);
        // Wait until loading loading screen is done
        yield return new WaitUntil(() => (asyncLoadingScreen.isDone));

        // Immediately load the target scene
        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync((int)SceneToLoad);
        while (!asyncLoadLevel.isDone)
        {
            // The loading stage is only calculated by Unity as a progress from 0 - 0.9
            // Progress from 0.9 - 1 is reserved for activating the scene, which is not needed for showing loading progress
            LoadProgress = Mathf.Clamp01(asyncLoadLevel.progress / 0.9f);

            yield return null;
        }

        yield return new WaitForEndOfFrame();
        LoadProgress = 0f;

        // Reset Timescale
        gameManager.gameState.ResumeGame();

        // Fade out loading screen for 1 second
        yield return StartCoroutine(FadeLoadingScreen(0f, 1f));
    }
    private IEnumerator LoadSceneCoroutine(string sceneName, float delay)
    {
        LoadProgress = 0f;
        SceneToLoad = (SceneName)SceneManager.GetSceneByName(sceneName).buildIndex;
        yield return new WaitForSecondsRealtime(delay);

        /** Transition animation / load screen
        // Transition Animation
        animator.SetTrigger("Entrance");

        yield return new WaitForSecondsRealtime(1f);
        */

        // Activate laoding screen
        loadTransitionCanvas.gameObject.SetActive(true);
        // Fade in loading screen for 1 second
        yield return StartCoroutine(FadeLoadingScreen(1f, 1f));
        // Load loading screen scene
        AsyncOperation asyncLoadingScreen = SceneManager.LoadSceneAsync((int)SceneName.LOADING_SCREEN);
        // Wait until loading loading screen is done
        yield return new WaitUntil(() => (asyncLoadingScreen.isDone));

        // Immediately load the target scene
        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoadLevel.isDone)
        {
            // The loading stage is only calculated by Unity as a progress from 0 - 0.9
            // Progress from 0.9 - 1 is reserved for activating the scene, which is not needed for showing loading progress
            LoadProgress = Mathf.Clamp01(asyncLoadLevel.progress / 0.9f);

            yield return null;
        }

        yield return new WaitForEndOfFrame();
        LoadProgress = 0f;

        // Reset Timescale
        gameManager.gameState.ResumeGame();

        // Fade out loading screen for 1 second
        yield return StartCoroutine(FadeLoadingScreen(0f, 1f));
    }
    #endregion

    #region Component Manager
    private void OnSceneLoaded()
    {
        SceneName currentScene = (SceneName)GetCurrentScene().buildIndex;

        switch (currentScene)
        {
            case SceneName.TITLE_SCREEN | SceneName.MAIN_HUB | SceneName.TEST_MISSION_SCENE | SceneName.LOADING_SCREEN:
                if (gameManager.gameState) gameManager.gameState.enabled = false;
                if (gameManager.gamePlayer) gameManager.gamePlayer.enabled = false;
                if (gameManager.gameEscortee) gameManager.gameEscortee.enabled = false;
                if (gameManager.gameWeapon) gameManager.gameWeapon.enabled = false;
                if (gameManager.gameEnemy) gameManager.gameEnemy.enabled = false;
                break;
            case SceneName.TEST_ESCORT_SCENE:
                if (gameManager.gameState) gameManager.gameState.enabled = true;
                if (gameManager.gamePlayer) gameManager.gamePlayer.enabled = true;
                if (gameManager.gameEscortee) gameManager.gameEscortee.enabled = true;
                if (gameManager.gameWeapon) gameManager.gameWeapon.enabled = true;
                if (gameManager.gameEnemy) gameManager.gameEnemy.enabled = true;

                // Find active in-game cameras & UI (if one exists)
                gameManager.FindActiveInGameCameras();
                gameManager.FindActiveInGameUI();
                // Try to initialize In-Game UI & Camera
                gameManager.TryInitializeInGameCameras();
                gameManager.TryInitializeInGameUI();

                // Find any preexisting players first
                gameManager.gamePlayer.FindPlayerInScene();

                // Find any preexisting escortees first
                gameManager.gameEscortee.FindEscorteeInScene();
                break;
            default:
                if (gameManager.gameState) gameManager.gameState.enabled = false;
                if (gameManager.gamePlayer) gameManager.gamePlayer.enabled = false;
                if (gameManager.gameEscortee) gameManager.gameEscortee.enabled = false;
                if (gameManager.gameInput) gameManager.gameInput.enabled = true;
                break;
        }
    }
    private void ManageGMComponents(Scene scene, LoadSceneMode loadSceneMode)
    {
        OnSceneLoaded();
    }
    #endregion
}
