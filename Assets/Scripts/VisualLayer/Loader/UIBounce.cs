using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBounce : MonoBehaviour
{
    public float bounceHeight = 30f; 
    public float bounceSpeed = 3f;    

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        float newY = Mathf.Sin(Time.time * bounceSpeed) * bounceHeight;
        transform.localPosition = new Vector3(startPos.x, startPos.y + newY, startPos.z);
    }
}
