using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugDisplayAmmo : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField ammoDisplay;
    [SerializeField]
    private AmmoType ammoType;

    // Start is called before the first frame update
    void Start()
    {
        ammoDisplay.characterValidation = TMP_InputField.CharacterValidation.Decimal;
    }

    // Update is called once per frame
    void Update()
    {
        string textToDisplay = $"{GameManager.Instance.LoadedGameData.ammo[ammoType].Amount}";

        // Update ammo counter text
        ammoDisplay.text = textToDisplay;
    }

    public void AddAmmo(int value = 10)
    {
        GameManager.Instance.LoadedGameData.ammo[ammoType].Amount += value;
    }
}
