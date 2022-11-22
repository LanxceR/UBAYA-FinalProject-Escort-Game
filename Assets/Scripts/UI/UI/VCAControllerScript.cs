using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VCAControllerScript : MonoBehaviour
{
    // Start is called before the first frame update
    private FMOD.Studio.VCA VcaController;
    public string VcaName;

    void Start()
    {
        VcaController = FMODUnity.RuntimeManager.GetVCA("vca:/" + VcaName);
    }

    public void SetVolume(float volume)
    {
        VcaController.setVolume(volume);
    }
}
