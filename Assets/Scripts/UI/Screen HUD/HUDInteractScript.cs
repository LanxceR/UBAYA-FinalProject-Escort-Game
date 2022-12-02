using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDInteractScript : MonoBehaviour
{
    public GameObject go;
    public TextMeshProUGUI mountText;

    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.gamePlayer.ActivePlayer.playerInteractScript.CheckIfInteractableExists() == true)
        {
            go.SetActive(true);
        }
        else
        {
            go.SetActive(false);
        }

        if(GameManager.Instance.gameEscortee.ActiveEscortee.escorteeInteractScript.CheckIfMounted() != true)
        {
            mountText.text = "Mount";
        }
        else
        {
            mountText.text = "Dismount";
        }
    }
}
