using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// The pause menu UI script (handles the pause menu UI)
/// </summary>
public class InGamePauseUIScript : MonoBehaviour
{
    // Reference to the main UI script
    [SerializeField]
    private InGameUIScript inGameUIScript;

    // Components
    [SerializeField]
    private GameObject pausePanel;
    [SerializeField]
    private GameObject settingsPanel;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("InGamePauseUIScript starting");

        // Subscribe methods to game manager
        GameManager.Instance.gameState.OnPauseAction += PauseGame;
        GameManager.Instance.gameState.OnResumeAction += ResumeGame;

        // Disable pause panel at start
        if (pausePanel)
            pausePanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    // Methods to invoke when pausing and resuming the game
    private void PauseGame()
    {
        // Enable pause panel
        if (pausePanel)
            pausePanel.SetActive(true);
    }
    private void ResumeGame()
    {
        // Disable pause panel
        if (pausePanel)
            pausePanel.SetActive(false);
        if (settingsPanel)
            settingsPanel.SetActive(false);
    }

    public void OnPointerOver()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Hover");
    }

    public void ResumeClick()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Click");
        ResumeGame();
        GameManager.Instance.GameIsPlaying = true;
    }

    public void OpenSettingsUI()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Click");
        settingsPanel.SetActive(true);
    }

    public void CloseSettingsUI()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Click");
        settingsPanel.SetActive(false);
    }

    // This function is called when the MonoBehaviour will be destroyed
    private void OnDestroy()
    {
        GameManager.Instance.gameState.OnPauseAction -= PauseGame;
        GameManager.Instance.gameState.OnResumeAction -= ResumeGame;
    }
}
