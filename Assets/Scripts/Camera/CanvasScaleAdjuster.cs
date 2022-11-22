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
    public Canvas canvas;
    public CanvasScaler canvasScaler;
    public PixelPerfectCamera pixelPerfectCamera;

    // Start is called before the first frame update
    void Start()
    {
        if (!pixelPerfectCamera && canvas.worldCamera)
        {
            canvas.worldCamera.TryGetComponent<PixelPerfectCamera>(out PixelPerfectCamera cam);
            pixelPerfectCamera = cam;
        }

        AdjustScalingFactor();
    }

    // LateUpdate is called every frame, if the Behaviour is enabled
    void LateUpdate()
    {
        AdjustScalingFactor();
    }

    void AdjustScalingFactor()
    {
        if (!pixelPerfectCamera) return;

        // Adjust scale factor to match Pixel Perfect Camera's pixel ratio
        canvasScaler.scaleFactor = pixelPerfectCamera.pixelRatio;
    }
}
