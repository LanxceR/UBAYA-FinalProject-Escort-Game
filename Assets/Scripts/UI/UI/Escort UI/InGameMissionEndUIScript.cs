using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The game over menu UI script (handles the game over menu UI)
/// </summary>
public class InGameMissionEndUIScript : MonoBehaviour
{
    // Reference to the main UI script
    [SerializeField]
    private InGameUIScript inGameUIScript;

    // Components
    [Header("Success Screen")]
    [SerializeField]
    private InGameMissionSuccessDisplayInfo missionSuccessPanel;
    [SerializeField]
    private GameObject missionFailedPanel;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("InGameGameOverUIScript starting");

        GameManager.Instance.gameMission.OnMissionEnd += MissionEnd;
    }

    // Methods
    private void MissionEnd(MissionEndEvent missionEndEvent, float reward)
    {        
        GameplayAudioManager audio = GameObject.FindObjectOfType<GameplayAudioManager>();
        if (audio)
            audio.KillAll();
        Cursor.visible = true;

        switch (missionEndEvent)
        {
            case MissionEndEvent.MISSION_SUCCESS:
                missionSuccessPanel.gameObject.SetActive(true);
                missionSuccessPanel.DisplayMissionEndDetail(reward);
                FMODUnity.RuntimeManager.PlayOneShot("event:/Music/Win");
                break;
            case MissionEndEvent.MISSION_FAILED:
                missionFailedPanel.SetActive(true);
                FMODUnity.RuntimeManager.PlayOneShot("event:/Music/Lose");
                break;
            default:
                break;
        }

        //Disable pause function.
        transform.GetComponent<InGamePauseUIScript>().enabled = false;
    }
    private void DisableGameOverPanel()
    {
        // Enable pause panel
        missionFailedPanel.SetActive(false);
    }

    // This function is called when the MonoBehaviour will be destroyed
    private void OnDestroy()
    {
        GameManager.Instance.gameMission.OnMissionEnd -= MissionEnd;
    }
}
