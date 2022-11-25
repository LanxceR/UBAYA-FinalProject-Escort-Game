using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SaveLoadUIScript : MonoBehaviour
{
    public int saveIndex;

    [Header("Buttons")]
    public GameObject loadButton;
    public GameObject saveButton;
    public GameObject deleteButton;

    [Header("Load Text")]
    public TextMeshProUGUI difficulty;
    public TextMeshProUGUI cashOwned;
    public TextMeshProUGUI ownedVehicles;
    public TextMeshProUGUI ownedWeapons;

    [Header("Load Text Dynamic")]
    public GameObject day;
    public GameObject missions;

    [Header("UI")]
    public GameObject saveLoadUI;
    public GameObject deleteUI;

    PlayerData[] gameDataList;

    // Start is called before the first frame update
    void Start()
    {
        Refresh();
        //if (GameManager.Instance.GameDatas[saveIndex].isEmpty == true)
        /*
        if (gameDataList[saveIndex] == null)
        {
            saveButton.SetActive(true);
            loadButton.SetActive(false);
            deleteButton.SetActive(false);
        }
        else
        {
            deleteButton.SetActive(true);

            difficulty.text = gameDataList[saveIndex].difficulty.ToString();
            cashOwned.text = "$" + gameDataList[saveIndex].money;
            ownedVehicles.text = gameDataList[saveIndex].ownedVehicles.Count.ToString() + "/3";
            ownedWeapons.text = gameDataList[saveIndex].ownedWeapons.Count.ToString() + "/8";

            if (gameDataList[saveIndex].difficulty == Difficulty.HARDCORE)
            {
                day.SetActive(true);
                Transform dayText = day.transform.Find("Text");
                dayText.GetComponent<TextMeshProUGUI>().text = gameDataList[saveIndex].daysPassed.ToString();

                missions.GetComponent<TextMeshProUGUI>().text = "Failed Missions";
                Transform missionCount = missions.transform.Find("Text");
                missionCount.GetComponent<TextMeshProUGUI>().text = gameDataList[saveIndex].missionsFailed.ToString();
            }
            else
            {
                day.SetActive(false);

                missions.GetComponent<TextMeshProUGUI>().text = "Missions Completed";
                Transform missionCount = missions.transform.Find("Text");
                missionCount.GetComponent<TextMeshProUGUI>().text = gameDataList[saveIndex].missionsCompleted.ToString();
            }
        }*/
    }

    void Refresh()
    {
        gameDataList = GameManager.Instance.gameData.LoadGamesFromFiles();

        if (gameDataList[saveIndex] == null)
        {
            saveButton.SetActive(true);
            loadButton.SetActive(false);
            deleteButton.SetActive(false);
        }
        else
        {
            deleteButton.SetActive(true);

            difficulty.text = gameDataList[saveIndex].difficulty.ToString();
            cashOwned.text = "$" + gameDataList[saveIndex].money;
            ownedVehicles.text = gameDataList[saveIndex].ownedVehicles.Count.ToString() + "/3";
            ownedWeapons.text = gameDataList[saveIndex].ownedWeapons.Count.ToString() + "/8";

            if (gameDataList[saveIndex].difficulty == Difficulty.HARDCORE)
            {
                day.SetActive(true);
                Transform dayText = day.transform.Find("Text");
                dayText.GetComponent<TextMeshProUGUI>().text = gameDataList[saveIndex].daysPassed.ToString();

                missions.GetComponent<TextMeshProUGUI>().text = "Failed Missions:";
                Transform missionCount = missions.transform.Find("Text");
                missionCount.GetComponent<TextMeshProUGUI>().text = gameDataList[saveIndex].missionsFailed.ToString();
            }
            else
            {
                day.SetActive(false);

                missions.GetComponent<TextMeshProUGUI>().text = "Missions Completed:";
                Transform missionCount = missions.transform.Find("Text");
                missionCount.GetComponent<TextMeshProUGUI>().text = gameDataList[saveIndex].missionsCompleted.ToString();
            }
        }

    }


    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    public void CreateSave()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Click");
        saveLoadUI.GetComponent<SaveLoadScript>().OpenDifficultyUI(saveIndex);
    }

    public void LoadSaveData()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Click");
        GameManager.Instance.gameData.LoadGame(saveIndex);
        GameManager.Instance.gameScene.GotoScene(SceneName.CUTSCENE);
    }

    public void OnPointerOver()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Hover");
    }

    public void OpenDeleteUI()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Click");
        deleteUI.SetActive(true);
        deleteButton.GetComponent<Button>().interactable = false;
    }

    public void ConfirmDelete()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Click");
        GameManager.Instance.gameData.DeleteSave(saveIndex);
        Refresh();
        deleteUI.SetActive(false);
    }

    public void CloseDeleteUI()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Click");
        deleteUI.SetActive(false);
        deleteButton.GetComponent<Button>().interactable = true;
    }
}
