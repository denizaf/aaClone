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
        if (reverseDirection)
        {
            _currentSpeed = -Mathf.Abs(speed);
        }
        else
        {
            _currentSpeed = Mathf.Abs(speed);
        }

        if (acceleration != 0)
        {
            _currentSpeed += acceleration * Time.deltaTime;
        }
        
        if (_isRotating)
        {
            transform.Rotate(0, 0, speed * Time.deltaTime);    
        }
    }

    public void StopRotation()
    {
        _isRotating = false;
    }
}
