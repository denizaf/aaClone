using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Pin : MonoBehaviour
{
    public float speed = 20f;
    public bool isInitialPin = false;
    
    private bool _isMoving = true;
    private bool _isPlaced = false;
    private Rigidbody2D _rb;
    private LineRenderer _lineRenderer;
    private Transform _circleTransform;
    private bool _collisionDetected = false;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _circleTransform = GameObject.FindGameObjectWithTag("Circle").transform;
    }

    private void Update()
    {
        if (isInitialPin && !_isPlaced)
        {
            // Do not move if it's an initial pin and not yet placed
            return;
        }
        
        if (_isMoving)
        {
            transform.Translate(Vector2.up * speed * Time.deltaTime);   
        }
        
        if (_lineRenderer != null)
        {
            _lineRenderer.SetPosition(0, _circleTransform.position);
            _lineRenderer.SetPosition(1, transform.position);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_collisionDetected) return; // Prevent multiple collisions
        _collisionDetected = true;
        
        _isMoving = false;
        _rb.velocity = Vector2.zero;
        _rb.isKinematic = true;

        if (collision.gameObject.CompareTag("Circle"))
        {
            if (!isInitialPin)
            {
                GameManager.Instance.PinLanded();   
            }
            AttachToCircle();
        }
        
        if (collision.gameObject.CompareTag("Pin"))
        {
            transform.SetParent(_circleTransform);
            // Change color of colliding pins
            GetComponent<SpriteRenderer>().color = Color.red;
            collision.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            
            //Handle game over logic,
            GameManager.Instance.CollisionOccurred(collision.contacts[0].point);
        }
    }
    
    public void PlacePin(Vector3 position)
    {
        transform.position = position;
        _isPlaced = true;  // Mark as placed
    }
    
    private void AttachToCircle()
    {
        transform.SetParent(_circleTransform);

        if (!isInitialPin)
        {
            LevelManager.Instance.PinAttached();
        }
        
        // Create and configure the LineRenderer
        _lineRenderer = gameObject.AddComponent<LineRenderer>();
        _lineRenderer.positionCount = 2;
        _lineRenderer.startWidth = 0.05f;
        _lineRenderer.endWidth = 0.05f;
        _lineRenderer.material = new Material(Shader.Find("Unlit/Color"));
        _lineRenderer.material.color = Color.black;  // Set the color of the line

        // Ensure the LineRenderer positions are updated
        _lineRenderer.SetPosition(0, _circleTransform.position);
        _lineRenderer.SetPosition(1, transform.position);
    }
}
