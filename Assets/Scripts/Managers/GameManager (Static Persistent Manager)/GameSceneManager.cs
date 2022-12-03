using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public enum SceneName
{
    LOADING_SCREEN,
    TITLE_SCREEN,
    SAVE_LOAD,
    CUTSCENE,
    MAIN_HUB,
    MAP_CITY,
    MAP_SUBURBS,
    MAP_FOREST,
    TEST_ESCORT_SCENE,
    TEST_MISSION_SCENE,
    PERMADEATH_SCREEN,
    ENDING_SCREEN
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
    [SerializeField] internal Canvas loadTransitionCanvas;
    [SerializeField] internal CanvasGroup loadTransitionCanvasGroup;

    // Variables
    [Header("Variables")]
    [SerializeField] private SceneName defaultSceneTarget;
    [SerializeField] private bool loadToDefaultAtStartup;

    public SceneName SceneToLoad { get; private set; }
    public float LoadProgress { get; private set; }

    // Variables
    private bool sceneLoaded;
    private Coroutine SceneLoadingCoroutine;
    private Coroutine LoadingFadeCoroutine;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // Manage GameManager's components at start
        //ManageGMComponents();

        //OnSceneChange += ManageGMComponents;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Start is called just before any of the Update methods is called the first time
    private void Start()
    {
        if (loadTransitionCanvasGroup)
        {
            loadTransitionCanvasGroup.alpha = 0f;
            loadTransitionCanvasGroup.gameObject.SetActive(false);
        }

        if (loadToDefaultAtStartup)
            GotoScene(defaultSceneTarget);
    }

    #region Main Functions
    // Go to scene
    public void GotoScene(SceneName scene)
    {
        if (SceneLoadingCoroutine == null)
            SceneLoadingCoroutine = StartCoroutine(LoadSceneCoroutine(scene, 0));
    }
    public void GotoScene(string sceneName)
    {
        if (SceneLoadingCoroutine == null)
            SceneLoadingCoroutine = StartCoroutine(LoadSceneCoroutine(sceneName, 0));
    }
    public void GotoScene()
    {
        if (SceneLoadingCoroutine == null)
            SceneLoadingCoroutine = StartCoroutine(LoadSceneCoroutine(defaultSceneTarget, 0));
    }

    // Go to scene with delay
    public void GotoSceneWithDelay(SceneName scene, float delay)
    {
        if (SceneLoadingCoroutine == null)
            SceneLoadingCoroutine = StartCoroutine(LoadSceneCoroutine(scene, delay));
    }
    public void GotoSceneWithDelay(string sceneName, float delay)
    {
        if (SceneLoadingCoroutine == null)
            SceneLoadingCoroutine = StartCoroutine(LoadSceneCoroutine(sceneName, delay));
    }
    public void GotoSceneWithDelay(float delay)
    {
        if (SceneLoadingCoroutine == null)
            SceneLoadingCoroutine = StartCoroutine(LoadSceneCoroutine(defaultSceneTarget, delay));
    }

    // Smoothly fade loading screen into the specified target alpha
    private IEnumerator FadeLoadingScreen(float targetAlpha, float duration)
    {
        float startAlpha = loadTransitionCanvasGroup.alpha;
        float timeElapsed = 0;
        while (timeElapsed < duration)
        {
            loadTransitionCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, timeElapsed / duration);
            timeElapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        loadTransitionCanvasGroup.alpha = targetAlpha;
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

        // Activate loading screen
        loadTransitionCanvasGroup.gameObject.SetActive(true);
        // Fade in loading screen for 1 second
        yield return StartCoroutine(FadeLoadingScreen(1f, 1f));

        // Load loading screen scene
        yield return StartCoroutine(LoadingScreenCoroutine());

        AssignTransitionRenderCamera();
        Debug.Log("Loading new scene...");

        // Immediately load the target scene
        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync((int)SceneToLoad);
        while (!asyncLoadLevel.isDone)
        {
            // The loading stage is only calculated by Unity as a progress from 0 - 0.9
            // Progress from 0.9 - 1 is reserved for activating the scene, which is not needed for showing loading progress
            LoadProgress = Mathf.Clamp01(asyncLoadLevel.progress / 0.9f);

            yield return null;
        }

        // Double check to wait until the scene has been loaded
        yield return new WaitUntil(() => sceneLoaded);
        // Immediately set the sceneLoaded flag to false
        sceneLoaded = false;

        Debug.Log($"{SceneToLoad.ToString()} loaded!");

        yield return new WaitForEndOfFrame();
        LoadProgress = 0f;

        // Stop all ongoing loading fade coroutines
        if (LoadingFadeCoroutine != null) StopCoroutine(LoadingFadeCoroutine);
        // Fade out loading screen for 1 second
        yield return LoadingFadeCoroutine = StartCoroutine(FadeLoadingScreen(0f, 1f));

        // Deactivate loading screen
        loadTransitionCanvasGroup.gameObject.SetActive(false);

        SceneLoadingCoroutine = null;
    }


    private IEnumerator LoadSceneCoroutine(string sceneName, float delay)
    {
        LoadProgress = 0f;
        SceneToLoad = (SceneName)SceneManager.GetSceneByName(sceneName).buildIndex;
        yield return new WaitForSecondsRealtime(delay);

        // Activate laoding screen
        loadTransitionCanvasGroup.gameObject.SetActive(true);
        // Fade in loading screen for 1 second
        yield return StartCoroutine(FadeLoadingScreen(1f, 1f));

        // Load loading screen scene
        yield return StartCoroutine(LoadingScreenCoroutine());

        AssignTransitionRenderCamera();
        Debug.Log("Loading new scene...");

        // Immediately load the target scene
        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoadLevel.isDone)
        {
            // The loading stage is only calculated by Unity as a progress from 0 - 0.9
            // Progress from 0.9 - 1 is reserved for activating the scene, which is not needed for showing loading progress
            LoadProgress = Mathf.Clamp01(asyncLoadLevel.progress / 0.9f);

            yield return null;
        }

        // Double check to wait until the scene has been loaded
        yield return new WaitUntil(() => sceneLoaded);
        // Immediately set the sceneLoaded flag to false
        sceneLoaded = false;

        Debug.Log($"{SceneToLoad.ToString()} loaded!");

        yield return new WaitForEndOfFrame();
        LoadProgress = 0f;

        // Stop all ongoing loading fade coroutines
        if (LoadingFadeCoroutine != null) StopCoroutine(LoadingFadeCoroutine);
        // Fade out loading screen for 1 second
        yield return LoadingFadeCoroutine = StartCoroutine(FadeLoadingScreen(0f, 1f));

        // Deactivate loading screen
        loadTransitionCanvasGroup.gameObject.SetActive(false);

        SceneLoadingCoroutine = null;
    }

    private IEnumerator LoadingScreenCoroutine()
    {
        SceneManager.LoadScene((int)SceneName.LOADING_SCREEN, LoadSceneMode.Additive);

        // Wait until the loading screen scene has been loaded
        yield return new WaitUntil(() => sceneLoaded);
        // Immediately set the sceneLoaded flag to false
        sceneLoaded = false;
    }
    #endregion

    #region Component Manager
    private void AssignTransitionRenderCamera()
    {
        loadTransitionCanvas.worldCamera = Camera.main;
        loadTransitionCanvas.sortingOrder = 10;
    }

    private void OnSceneLoaded()
    {
        SceneName currentScene = (SceneName)GetCurrentScene().buildIndex;

        if (gameManager.gameMission.escortScenes.Contains(currentScene))
        {
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
        }
        else
        {
            if (gameManager.gameState) gameManager.gameState.enabled = false;
            if (gameManager.gamePlayer) gameManager.gamePlayer.enabled = false;
            if (gameManager.gameEscortee) gameManager.gameEscortee.enabled = false;
            if (gameManager.gameWeapon) gameManager.gameWeapon.enabled = false;
            if (gameManager.gameEnemy) gameManager.gameEnemy.enabled = false;
            if (gameManager.gameInput) gameManager.gameInput.enabled = true;
        }

        // Reset Timescale
        gameManager.gameState.canPauseAndResume = true;
        gameManager.gameState.ResumeGame();

        // Set sceneLoaded flag to true momentarily
        sceneLoaded = true;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        OnSceneLoaded();
    }
    #endregion
}
