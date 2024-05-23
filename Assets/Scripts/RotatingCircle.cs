using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class RotatingCircle : MonoBehaviour
{
    public float speed = 100f;
    public float acceleration;
    public bool reverseDirection;
    
    private bool _isRotating = true;
    private float _currentSpeed;

    private void Start()
    {
        _currentSpeed = speed;
    }

    private void Update()
    {
        if (_isRotating)
        {
            float rotationSpeed = reverseDirection ? -Mathf.Abs(_currentSpeed) : Mathf.Abs(_currentSpeed);
            _currentSpeed += acceleration * Time.deltaTime;
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }
    }

    public void StopRotation()
    {
        _isRotating = false;
    }
}
