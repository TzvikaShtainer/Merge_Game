using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScaler : MonoBehaviour
{
    public float baseWidth = 1080f; // Set your base resolution width
    public float baseHeight = 1920f; // Set your base resolution height
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        AdjustCamera();
    }

    void AdjustCamera()
    {
        float targetAspect = baseWidth / baseHeight;
        float screenAspect = (float)Screen.width / Screen.height;

        if (screenAspect >= targetAspect)
        {
            cam.orthographicSize = baseHeight / 200f; // Adjust scale
        }
        else
        {
            cam.orthographicSize = (baseWidth / screenAspect) / 200f;
        }
    }
}
