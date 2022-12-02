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
        slider = this.transform.GetComponent<Slider>();

        VcaController = FMODUnity.RuntimeManager.GetVCA("vca:/" + VcaName);

        if (PlayerPrefs.HasKey(VcaName))
        {
            VcaController.setVolume(PlayerPrefs.GetFloat(VcaName));
        }
        else
        {
            VcaController.setVolume(1f);
        }
        slider.value = PlayerPrefs.GetFloat(VcaName);
    }

    public void SetVolume(float volume)
    {
        VcaController.setVolume(volume);
        PlayerPrefs.SetFloat(VcaName, volume);
        PlayerPrefs.GetFloat(VcaName);
    }
}
