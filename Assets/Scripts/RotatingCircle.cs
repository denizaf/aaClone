using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingCircle : MonoBehaviour
{
    public float rotationSpeed = 100f;
    private bool _isRotating = true;

    private void Update()
    {
        if (_isRotating)
        {
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);    
        }
    }

    public void StopRotation()
    {
        _isRotating = false;
    }
}
