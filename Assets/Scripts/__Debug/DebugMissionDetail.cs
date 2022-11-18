using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugMissionDetail : MonoBehaviour
{
    [SerializeField]
    internal TMP_Dropdown vehicle;
    [SerializeField]
    internal Toggle hasWpnToggle;

    [SerializeField]
    internal TextMeshProUGUI zCount;
    [SerializeField]
    internal TextMeshProUGUI baseReward;
    [SerializeField]
    internal TextMeshProUGUI enemies;

    [SerializeField]
    internal TMP_Dropdown melee;
    [SerializeField]
    internal TMP_Dropdown ranged1;
    [SerializeField]
    internal TMP_Dropdown ranged2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
