using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;

/// <summary>
/// Automatically adjust canvas scaler scale factor according to different resolutions
/// </summary>

[ExecuteInEditMode]
public class CanvasScaleAdjuster : MonoBehaviour
{
    // Variables
    public CanvasScaler CanvasScaler;
    private PixelPerfectCamera MainPixelPerfectCamera;

    // Start is called before the first frame update
    void Start()
    {
        MainPixelPerfectCamera = Camera.main.GetComponent<PixelPerfectCamera>();

        AdjustScalingFactor();
    }

    // LateUpdate is called every frame, if the Behaviour is enabled
    void LateUpdate()
    {
        AdjustScalingFactor();
    }

    void AdjustScalingFactor()
    {
        // Adjust scale factor to match Pixel Perfect Camera's pixel ratio
        CanvasScaler.scaleFactor = MainPixelPerfectCamera.pixelRatio;
    }
}
