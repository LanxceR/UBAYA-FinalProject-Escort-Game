using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SaveLoadScript : MonoBehaviour
{
    [Header("Difficulty UI")]
    public GameObject difficultyUI;
    private int currentIndex;

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    public void OpenDifficultyUI(int index)
    {
        currentIndex = index;
        difficultyUI.SetActive(true);
    }

    public void CloseDifficultyUI()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Click");
        difficultyUI.SetActive(false);
    }

    public void SaveCasual()
    {
        CreateSave(Difficulty.CASUAL);
    }

    public void SaveHardcore()
    {
        CreateSave(Difficulty.HARDCORE);
    }

    void CreateSave(Difficulty diff)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Click");
        GameManager.Instance.gameData.CreateGame(currentIndex, diff);
        GameManager.Instance.gameData.LoadGame(currentIndex);
        GameManager.Instance.gameData.SaveGame(currentIndex);
        GameManager.Instance.gameScene.GotoScene(SceneName.CUTSCENE);
    }

    public void OnPointerOver()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Hover");
    }
}
