using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VCAControllerScript : MonoBehaviour
{
    // Start is called before the first frame update
    private FMOD.Studio.VCA VcaController;
    public string VcaName;

    private Slider slider;

    void Start()
    {
        VcaController = FMODUnity.RuntimeManager.GetVCA("vca:/" + VcaName);
        slider = GetComponent<Slider>();
    }

    public void SetVolume(float volume)
    {
        VcaController.setVolume(volume);
    }
}
