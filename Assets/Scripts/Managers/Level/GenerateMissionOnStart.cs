using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMissionOnStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.gameMission.GenerateMissions(GameManager.Instance.LoadedGameData.daysPassed);
    }
}
