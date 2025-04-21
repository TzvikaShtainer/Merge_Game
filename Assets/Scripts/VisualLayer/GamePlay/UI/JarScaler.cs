using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JarScaler : MonoBehaviour
{
    [SerializeField]
    private float scaleFactor = 0.95f;
    private void Start()
    {
        FitToScreen();
    }
    private void FitToScreen()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        if (sr == null || sr.sprite == null) return;

        float screenHeight = Camera.main.orthographicSize * 2;
        float screenWidth = screenHeight * Screen.width / Screen.height;

        Vector2 spriteSize = sr.sprite.bounds.size;

        float scaleX = screenWidth / spriteSize.x;
        float scaleY = screenHeight / spriteSize.y;

        
        transform.localScale = new Vector3(scaleX * scaleFactor, scaleY * scaleFactor, 1);
    }
}
